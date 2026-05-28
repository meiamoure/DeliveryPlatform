using System;
using DeliveryPlatform.Core.Domain.Vehicles.Common;

namespace DeliveryPlatform.Api.Controllers.Vehicles.Requests;

public sealed record SetVehicleStatusRequest(
    VehicleStatus Status
);
