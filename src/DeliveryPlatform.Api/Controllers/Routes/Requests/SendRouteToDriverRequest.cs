using System;

namespace DeliveryPlatform.Api.Controllers.Routes.Requests;

public sealed record SendRouteToDriverRequest(Guid DriverId);
