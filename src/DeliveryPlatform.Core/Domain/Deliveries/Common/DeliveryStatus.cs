using System;

namespace DeliveryPlatform.Core.Domain.Deliveries.Common;

public enum DeliveryStatus
{
    Pending,
    Planned,
    InProgress,
    Delivered,
    Cancelled
}
