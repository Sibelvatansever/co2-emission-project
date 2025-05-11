using Microsoft.AspNetCore.Mvc;
using SharedContracts.Dtos;

namespace MeasurementsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementsController : ControllerBase
{
    private static readonly Random _random = new();

    [HttpGet("{userId}")]
    public IActionResult GetMeasurements(string userId, [FromQuery] long from, [FromQuery] long to)
    {
        if (_random.NextDouble() < 0.3) // chaos
            return StatusCode(500, "Temporary error. Try again.");

        var resolutionInSeconds = userId switch
        {
            "alpha" => 3,
            "beta" => 4,
            "gamma" => 5,
            "delta" => 6,
            _ => 10
        };

        var measurements = new List<MeasurementDto>();

        for (var timestamp = AlignTimestamp(from, resolutionInSeconds); timestamp <= to; timestamp += resolutionInSeconds)
        {
            measurements.Add(new MeasurementDto
            {
                Timestamp = timestamp,
                Watts = _random.Next(200, 800)
            });
        }

        return Ok(measurements);
    }

    private static long AlignTimestamp(long timestamp, int resolution)
    {
        return timestamp + (resolution - (timestamp % resolution));
    }
}