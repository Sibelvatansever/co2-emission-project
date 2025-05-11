using Microsoft.AspNetCore.Mvc;
using SharedContracts.Dtos;

namespace EmissionsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmissionsController : ControllerBase
{
    private static readonly Random _random = new();

    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetEmissions([FromQuery] long from, [FromQuery] long to)
    {
        if (from > to)
            return BadRequest("Invalid time range.");

        if (_random.NextDouble() < 0.5)
            await Task.Delay(15000); // In case of CHAOS slow response

        var emissions = new List<EmissionDto>();

        for (var timestamp = AlignTo15Min(from); timestamp <= to; timestamp += 900)
        {
            emissions.Add(new EmissionDto
            {
                Timestamp = timestamp,
                Factor = Math.Round(_random.NextDouble() * (0.5 - 0.1) + 0.1, 3)
            });
        }

        return Ok(emissions);
    }
    private static long AlignTo15Min(long timestamp)
    {
        const int interval = 900;
        return timestamp + (interval - (timestamp % interval));
    }
}