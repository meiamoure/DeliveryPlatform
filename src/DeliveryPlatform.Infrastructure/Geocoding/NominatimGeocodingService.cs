using System.Net.Http.Json;
using DeliveryPlatform.Application.Common.Geocoding;
using System.Text.Json;

namespace DeliveryPlatform.Infrastructure.Geocoding;

public class NominatimGeocodingService : IGeocodingService
{
    private readonly HttpClient _http;

    public NominatimGeocodingService(HttpClient http)
    {
        _http = http;
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("DeliveryPlatform/1.0");
    }

    public async Task<(double lat, double lng)> GetCoordinatesAsync(string address)
    {
        var url =
            $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";

        await Task.Delay(1000);

        //var response = await _http.GetFromJsonAsync<List<NominatimResponse>>(url);

        var httpResponse = await _http.GetAsync(url);

        var content = await httpResponse.Content.ReadAsStringAsync();

        Console.WriteLine("STATUS: " + httpResponse.StatusCode);
        Console.WriteLine("BODY: " + content);

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception($"HTTP ERROR: {httpResponse.StatusCode}");

        var response = JsonSerializer.Deserialize<List<NominatimResponse>>(content);

        if (response == null || response.Count == 0)
            throw new Exception("Address not found");

        var r = response[0];

        return (
            double.Parse(r.lat, System.Globalization.CultureInfo.InvariantCulture),
            double.Parse(r.lon, System.Globalization.CultureInfo.InvariantCulture)
        );
    }

    private class NominatimResponse
    {
        public string lat { get; set; } = "";
        public string lon { get; set; } = "";
    }
}
