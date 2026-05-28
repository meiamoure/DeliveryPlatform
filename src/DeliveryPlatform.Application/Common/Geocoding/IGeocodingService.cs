using System;

namespace DeliveryPlatform.Application.Common.Geocoding;

public interface IGeocodingService
{
    Task<(double lat, double lng)> GetCoordinatesAsync(string address);
}
