using System;

namespace DeliveryPlatform.Api.Controllers.Deliveries.Requests;

public sealed class ImportDeliveriesRequest
{
    public IFormFile File { get; set; } = default!;
}
