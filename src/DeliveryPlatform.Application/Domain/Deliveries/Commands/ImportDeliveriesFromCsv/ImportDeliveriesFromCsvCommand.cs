using System;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Deliveries.Commands.ImportDeliveriesFromCsv;

public sealed record ImportDeliveriesFromCsvCommand(
    Stream CsvStream
) : IRequest<int>;
