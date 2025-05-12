using Microsoft.AspNetCore.Mvc;
using SharedContracts.Dtos;

namespace CalculatorApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CalculatorController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Calculate([FromQuery] string userId, [FromQuery] long from, [FromQuery] long to)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var measurements = await GetMeasurementsWithRetryAsync(httpClient, userId, from, to);
        if (measurements == null || !measurements.Any())
            return StatusCode(503, "Measurement service unavailable.");

        var emissions = await GetEmissionsAsync(httpClient, from, to);
        if (emissions == null || !emissions.Any())
            return StatusCode(503, "Emissions service unavailable.");

        var grouped = measurements
            .GroupBy(m => m.Timestamp - (m.Timestamp % 900))
            .ToDictionary(g => g.Key, g => g.Average(m => m.Watts));

        double totalKg = 0;

        foreach (var (timestamp, avgWatts) in grouped)
        {
            var factor = emissions.FirstOrDefault(e => e.Timestamp == timestamp)?.Factor ?? 0;
            var kWh = avgWatts / 4.0 / 1000.0;
            totalKg += kWh * factor;
        }

        return Ok(new { totalEmissionKg = Math.Round(totalKg, 4) });
    }

    private async Task<List<MeasurementDto>?> GetMeasurementsWithRetryAsync(HttpClient client, string userId, long from, long to)
    {
        const int maxAttempts = 3;
        for (int i = 0; i < maxAttempts; i++)
        {
            try
            {
                var url = $"http://measurements-api/api/measurements/{userId}?from={from}&to={to}";
                var result = await client.GetFromJsonAsync<List<MeasurementDto>>(url);

                if (result != null && result.Any())
                    return result;
            }
            catch
            {
                await Task.Delay(500);
            }
        }

        return null;
    }

    private async Task<List<EmissionDto>?> GetEmissionsAsync(HttpClient client, long from, long to)
    {
        try
        {
            var url = $"http://emissions-api/api/emissions?from={from}&to={to}";
            return await client.GetFromJsonAsync<List<EmissionDto>>(url);
        }
        catch
        {
            return null;
        }
    }
}