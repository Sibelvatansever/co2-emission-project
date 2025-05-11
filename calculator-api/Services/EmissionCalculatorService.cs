using SharedContracts.Dtos;

namespace CalculatorApi.Services;

public class EmissionCalculatorService
{
    public double Calculate(Dictionary<long, double> avgWattsPerInterval, List<EmissionDto> emissions)
    {
        double totalKg = 0;

        foreach (var (timestamp, avgWatts) in avgWattsPerInterval)
        {
            var factor = emissions.FirstOrDefault(e => e.Timestamp == timestamp)?.Factor ?? 0;
            var kWh = avgWatts / 4.0 / 1000.0;
            totalKg += kWh * factor;
        }

        return Math.Round(totalKg, 4);
    }
}