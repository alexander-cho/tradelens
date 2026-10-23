using AwesomeAssertions;
using Tradelens.Fundamentals.Domain.Observations;

namespace Tradelens.Fundamentals.Domain.UnitTests.Observations;

public class ObservationTests
{
    [Fact]
    public void ObservationTest1()
    {
        var sofiQ2Fy2026Revenue = Observation.Create("us-gaap:RevenuesNetOfInterestExpense", "Revenue", 1_218_676_000);
        var sofiQ1Fy2026NcoRate = Observation.Create(null, "Net Charge-off Rate", 0.12);
        
        sofiQ2Fy2026Revenue.FlagObservation();

        sofiQ2Fy2026Revenue.GetEvents().Count.Should().Be(1);
    }
}