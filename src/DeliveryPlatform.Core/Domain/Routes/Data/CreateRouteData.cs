namespace DeliveryPlatform.Core.Domain.Routes.Data;

public record CreateRouteData(Guid VehicleId, DateOnly ServiceDate, Guid? DriverId = null);

