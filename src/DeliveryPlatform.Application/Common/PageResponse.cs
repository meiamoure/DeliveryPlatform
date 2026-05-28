using System;

namespace DeliveryPlatform.Application.Common;

public record PageResponse<T>(
    int Total,
    T Data)
    where T : class;
