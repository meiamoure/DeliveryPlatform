using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Routes.Data;

namespace DeliveryPlatform.Core.Domain.Routes.Models;

public class Route : IAggregateRoot
{
    private readonly List<RouteSegment> _segments = new();
    private Route() { }

    internal Route(Guid id, Guid vehicleId, DateOnly serviceDate, Guid? driverId)
    {
        Id = id;
        VehicleId = vehicleId;
        ServiceDate = serviceDate;
        DriverId = driverId;
        Status = RouteStatus.Planned;
    }

    public Guid Id { get; private set; }
    public Guid VehicleId { get; private set; }
    public Guid? DriverId { get; private set; }
    public DateOnly ServiceDate { get; private set; }
    public RouteStatus Status { get; private set; }
    public int Number { get; private set; }
    public string Code { get; private set; } = string.Empty;

    public IReadOnlyCollection<RouteSegment> Segments => _segments.AsReadOnly();

    public static Route Create(CreateRouteData d) =>
        new(Guid.NewGuid(), d.VehicleId, d.ServiceDate, d.DriverId);

    public void Update(UpdateRouteData d)
    {
        Status = d.Status;
        DriverId = d.DriverId;
    }

    public RouteSegment AddSegment(CreateRouteSegmentData d)
    {
        if (_segments.Any(s => s.Order == d.Order))
            throw new InvalidOperationException("Duplicate segment order");

        var seg = RouteSegment.Create(this, d);
        _segments.Add(seg);
        return seg;
    }

    public void RemoveSegment(Guid segmentId)
    {
        var seg = _segments.FirstOrDefault(x => x.Id == segmentId)
            ?? throw new InvalidOperationException("Segment not found");
        _segments.Remove(seg);
    }

    public void Accept()
    {
        if (Status != RouteStatus.Planned)
            throw new InvalidOperationException($"Only planned route can be accepted. Current status: {Status}");

        Status = RouteStatus.Accepted;
    }

    public void Start()
    {
        if (Status != RouteStatus.Accepted)
            throw new InvalidOperationException($"Only accepted route can be started. Current status: {Status}");

        Status = RouteStatus.InProgress;
    }

    public void Complete()
    {
        if (Status != RouteStatus.InProgress)
            throw new InvalidOperationException($"Only in-progress route can be completed. Current status: {Status}");

        Status = RouteStatus.Completed;
    }

    public void SetNumber(int number)
    {
        if (Number != 0)
            throw new InvalidOperationException("Route number already set");

        Number = number;
    }

    public void SetCode(string code)
    {
        if (!string.IsNullOrEmpty(Code))
            throw new InvalidOperationException("Route code already set");

        Code = code;
    }

    public double TotalDistanceKm => _segments.Sum(s => s.DistanceKm);
    public int TotalTimeMin => _segments.Sum(s => s.TravelTimeMin);
}
