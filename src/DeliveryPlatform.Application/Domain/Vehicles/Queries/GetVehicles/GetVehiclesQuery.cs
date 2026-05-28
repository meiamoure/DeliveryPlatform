using MediatR;
using DeliveryPlatform.Application.Domain.Vehicles.Queries.Dtos;

namespace DeliveryPlatform.Application.Domain.Vehicles.Queries.GetVehicles;

public sealed record GetVehiclesQuery() : IRequest<IReadOnlyList<VehicleDto>>;
