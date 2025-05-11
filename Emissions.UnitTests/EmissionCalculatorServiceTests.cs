using CalculatorApi.Services;
using SharedContracts.Dtos;
using Xunit;

namespace Emissions.UnitTests;

public class EmissionCalculatorServiceTests
{
    [Fact]
    public void Calculate_WithValidData_ReturnsCorrectEmission()
    {
        var service = new EmissionCalculatorService();

        var grouped = new Dictionary<long, double>
        {
            { 1000, 2000 }, // 2000 watt
            { 2001, 3000 }  // 3000 watt
        };

        var emissions = new List<EmissionDto>
        {
            new() { Timestamp = 1000, Factor = 0.3 },
            new() { Timestamp = 2001, Factor = 0.2 }
        };

        var result = service.Calculate(grouped, emissions);

        // Hesap: (2000/4/1000)*0.3 + (3000/4/1000)*0.2 = 0.15 + 0.15 = 0.3
        Assert.Equal(0.3, result);
    }
}
