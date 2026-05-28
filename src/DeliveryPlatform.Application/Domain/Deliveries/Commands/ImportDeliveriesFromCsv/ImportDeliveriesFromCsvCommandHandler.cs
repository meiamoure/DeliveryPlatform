using System.Globalization;
using MediatR;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Domain.Nodes.Common;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Deliveries.Data;
using DeliveryPlatform.Core.Domain.Deliveries.Models;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DeliveryPlatform.Core.Exceptions;

namespace DeliveryPlatform.Application.Domain.Deliveries.Commands.ImportDeliveriesFromCsv;

public sealed class ImportDeliveriesFromCsvCommandHandler
    : IRequestHandler<ImportDeliveriesFromCsvCommand, int>
{
    private readonly IDeliveryRepository _deliveries;
    private readonly INodeRepository _nodes;
    private readonly IUnitOfWork _uow;

    public ImportDeliveriesFromCsvCommandHandler(
        IDeliveryRepository deliveries,
        INodeRepository nodes,
        IUnitOfWork uow)
    {
        _deliveries = deliveries;
        _nodes = nodes;
        _uow = uow;
    }

    public async Task<int> Handle(
    ImportDeliveriesFromCsvCommand request,
    CancellationToken ct)
    {
        using var reader = new StreamReader(request.CsvStream, leaveOpen: true);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            PrepareHeaderForMatch = args => args.Header?.Trim(),
            MissingFieldFound = null,
            BadDataFound = null
        };

        using var csv = new CsvReader(reader, config);

        csv.Context.TypeConverterCache.AddConverter<TimeOnly>(new TimeOnlyCsvConverter());

        var records = csv.GetRecords<CsvDeliveryRecord>().ToList();

        var imported = 0;

        foreach (var rec in records)
        {
            ct.ThrowIfCancellationRequested();

            var delivery = await BuildDeliveryFromRecord(rec, ct);

            await _deliveries.AddAsync(delivery, ct);

            imported++;
        }

        await _uow.SaveChangesAsync(ct);

        return imported;
    }

    private async Task<Delivery> BuildDeliveryFromRecord(CsvDeliveryRecord rec, CancellationToken ct)
    {
        var node = await _nodes.GetByNameAsync(rec.Address, ct);

        if (node == null)
            throw new DeliveryPlatform.Core.Exceptions.ValidationException($"Node not found: {rec.Address}");

        if (!rec.WindowStart.HasValue || !rec.WindowEnd.HasValue)
            throw new DeliveryPlatform.Core.Exceptions.ValidationException("WindowStart and WindowEnd required");

        if (rec.WindowEnd <= rec.WindowStart)
            throw new DeliveryPlatform.Core.Exceptions.ValidationException("WindowEnd must be after WindowStart");

        // 🔥 SEQUENCE
        var number = await _deliveries.GetNextDeliveryNumber(ct);
        var orderNumber = $"D-{number:D3}";

        var createData = new CreateDeliveryData(
            OrderNumber: orderNumber,
            NodeId: node.Id,
            WeightKg: rec.WeightKg,
            VolumeM3: rec.VolumeM3,
            ProductGroup: (ProductGroup)rec.ProductGroup,
            WindowStart: rec.WindowStart.Value,
            WindowEnd: rec.WindowEnd.Value,
            Priority: (DeliveryPriority)rec.Priority,
            PickupNodeId: null
        );

        return Delivery.Create(createData);
    }

    private sealed class CsvDeliveryRecord
    {
        public string Address { get; set; } = null!;

        public decimal WeightKg { get; set; }
        public decimal VolumeM3 { get; set; }
        public int ProductGroup { get; set; }

        public TimeOnly? WindowStart { get; set; }
        public TimeOnly? WindowEnd { get; set; }

        public int Priority { get; set; }
    }

    private sealed class TimeOnlyCsvConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            if (TimeOnly.TryParseExact(text.Trim(), "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var t))
                return t;
            throw new TypeConverterException(this, memberMapData, text, row.Context, $"Invalid time format '{text}' (expected HH:mm)");
        }
    }
}
