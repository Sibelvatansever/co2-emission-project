using Xunit;

namespace Emissions.UnitTests
{
    public class EmissionCalculatorTests
    {
        [Fact]
        public void SampleTest_ShouldPass()
        {
            var expected = 2 + 2;

            var result = 4;

            Assert.Equal(expected, result);
        }
    }
}