using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using DeliveryPlatform.Application.Routing;
using DeliveryPlatform.Application.Routing.DistanceMatrixBuilder;

namespace DeliveryPlatform.Infrastructure.Routing.Osm;

public sealed class OsmRoutingService : IOsmRoutingService
{
    private readonly HttpClient _http;

    public OsmRoutingService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("osrm");
    }

    public async Task<(double DistanceKm, double DurationMin)> GetRouteAsync(
        double fromLat, double fromLng,
        double toLat, double toLng,
        CancellationToken ct)
    {
        Console.WriteLine(
            $"RAW INPUT >>> from=({fromLat},{fromLng}) to=({toLat},{toLng})");

        var url = string.Format(
            CultureInfo.InvariantCulture,
            "route/v1/driving/{0},{1};{2},{3}?overview=false",
            fromLng.ToString(CultureInfo.InvariantCulture),
            fromLat.ToString(CultureInfo.InvariantCulture),
            toLng.ToString(CultureInfo.InvariantCulture),
            toLat.ToString(CultureInfo.InvariantCulture));

        Console.WriteLine("OSRM REQUEST: " + url);

        var resp = await _http.GetAsync(url, ct);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync(ct);
        OsmRouteResponse? data;
        try
        {
            data = JsonSerializer.Deserialize<OsmRouteResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"OSRM invalid JSON. Url={url}. Body={json}", ex);
        }

        if (data is null)
            throw new InvalidOperationException($"OSRM deserialize returned null. Url={url}. Body={json}");

        var wp0 = data.Waypoints?[0];
        var wp1 = data.Waypoints?[1];
        Console.WriteLine($"SNAP0: lat={wp0?.Location?[1]} lon={wp0?.Location?[0]} distToRoadM={wp0?.Distance} name={wp0?.Name}");
        Console.WriteLine($"SNAP1: lat={wp1?.Location?[1]} lon={wp1?.Location?[0]} distToRoadM={wp1?.Distance} name={wp1?.Name}");

        if (!string.Equals(data.Code, "Ok", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"OSRM bad code: '{data.Code}'. Url={url}. Body={json}");

        if (data.Routes is null || data.Routes.Count == 0)
            throw new InvalidOperationException($"OSRM returned empty routes. Url={url}. Body={json}");

        var route = data.Routes[0];

        if (route.Legs is null || route.Legs.Count == 0)
            throw new InvalidOperationException($"OSRM route has no legs. Url={url}. Body={json}");

        var leg = route.Legs[0];

        if (double.IsNaN(leg.Distance) || double.IsInfinity(leg.Distance) || leg.Distance <= 0)
            throw new InvalidOperationException($"OSRM invalid distance={leg.Distance}. Url={url}. Body={json}");

        if (double.IsNaN(leg.Duration) || double.IsInfinity(leg.Duration) || leg.Duration <= 0)
            throw new InvalidOperationException($"OSRM invalid duration={leg.Duration}. Url={url}. Body={json}");

        return (leg.Distance / 1000.0, leg.Duration / 60.0);
    }

    public async Task<OsrmTableResult> GetTableAsync(
     List<(double lat, double lng)> coords,
     CancellationToken ct)
    {
        var coordinates = string.Join(";",
            coords.Select(c =>
                $"{c.lng.ToString(CultureInfo.InvariantCulture)},{c.lat.ToString(CultureInfo.InvariantCulture)}"));

        var url = $"table/v1/driving/{coordinates}?annotations=duration,distance";

        Console.WriteLine($"OSRM TABLE URL: {url}");

        var httpResponse = await _http.GetAsync(url, ct);
        var body = await httpResponse.Content.ReadAsStringAsync(ct);

        Console.WriteLine($"OSRM STATUS: {(int)httpResponse.StatusCode}");
        Console.WriteLine($"OSRM BODY: {body}");

        httpResponse.EnsureSuccessStatusCode();

        OsrmTableResponse? response;
        try
        {
            response = JsonSerializer.Deserialize<OsrmTableResponse>(
                body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"OSRM table invalid JSON. Url={url}. Body={body}", ex);
        }

        if (response is null)
            throw new InvalidOperationException($"OSRM table deserialize returned null. Url={url}. Body={body}");

        if (!string.Equals(response.Code, "Ok", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"OSRM table bad code: '{response.Code}'. Url={url}. Body={body}");

        if (response.Durations is null)
            throw new InvalidOperationException("OSRM table failed: durations is null");

        if (response.Distances is null)
            throw new InvalidOperationException("OSRM table failed: distances is null");

        return new OsrmTableResult(response.Durations, response.Distances);
    }

    public async Task<IReadOnlyList<(double lat, double lng)>> GetRouteGeometryAsync(
    List<(double lat, double lng)> coords,
    CancellationToken ct)
    {
        if (coords.Count < 2)
            return Array.Empty<(double lat, double lng)>();

        var coordinates = string.Join(";",
            coords.Select(c =>
                $"{c.lng.ToString(CultureInfo.InvariantCulture)},{c.lat.ToString(CultureInfo.InvariantCulture)}"));

        var url = $"route/v1/driving/{coordinates}?overview=full&geometries=geojson";

        Console.WriteLine($"OSRM GEOMETRY URL: {url}");

        var httpResponse = await _http.GetAsync(url, ct);
        var body = await httpResponse.Content.ReadAsStringAsync(ct);

        Console.WriteLine($"OSRM GEOMETRY STATUS: {(int)httpResponse.StatusCode}");
        Console.WriteLine($"OSRM GEOMETRY BODY: {body}");

        httpResponse.EnsureSuccessStatusCode();

        OsmRouteGeometryResponse? response;
        try
        {
            response = JsonSerializer.Deserialize<OsmRouteGeometryResponse>(
                body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"OSRM geometry invalid JSON. Url={url}. Body={body}", ex);
        }

        if (response is null)
            throw new InvalidOperationException($"OSRM geometry deserialize returned null. Url={url}. Body={body}");

        if (!string.Equals(response.Code, "Ok", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"OSRM geometry bad code: '{response.Code}'. Url={url}. Body={body}");

        if (response.Routes is null || response.Routes.Count == 0)
            throw new InvalidOperationException($"OSRM geometry returned empty routes. Url={url}. Body={body}");

        var geometry = response.Routes[0].Geometry;

        if (geometry?.Coordinates is null || geometry.Coordinates.Count == 0)
            throw new InvalidOperationException($"OSRM geometry coordinates are empty. Url={url}. Body={body}");

        return geometry.Coordinates
            .Select(c => (lat: c[1], lng: c[0]))
            .ToList();
    }

    private sealed class OsrmTableResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("durations")]
        public double[][] Durations { get; set; } = default!;

        [JsonPropertyName("distances")]
        public double[][] Distances { get; set; } = default!;
    }
}

public class OsmRouteResponse
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("routes")]
    public List<OsmRoute>? Routes { get; set; }

    [JsonPropertyName("waypoints")]
    public List<OsmWaypoint>? Waypoints { get; set; }
}

public class OsmRoute
{
    [JsonPropertyName("legs")]
    public List<OsmLeg>? Legs { get; set; }
}

public class OsmLeg
{
    [JsonPropertyName("distance")]
    public double Distance { get; set; }

    [JsonPropertyName("duration")]
    public double Duration { get; set; }
}

public class OsmWaypoint
{
    public double[]? Location { get; set; }
    public double Distance { get; set; }
    public string? Name { get; set; }
}

public class OsmRouteGeometryResponse
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("routes")]
    public List<OsmGeometryRoute>? Routes { get; set; }
}

public class OsmGeometryRoute
{
    [JsonPropertyName("geometry")]
    public OsmGeoJsonGeometry? Geometry { get; set; }
}

public class OsmGeoJsonGeometry
{
    [JsonPropertyName("coordinates")]
    public List<List<double>> Coordinates { get; set; } = [];
}
