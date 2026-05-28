using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Core.Domain.Routes.Data;

public record UpdateRouteData(RouteStatus Status, Guid? DriverId);

