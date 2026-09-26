using AwesomeAssertions;

namespace Tradelens.CompanyFundamentals.Domain.UnitTests;

public class LineItemTests
{
    [Fact]
    public void LineItemTest1()
    {
        var sofiQ2Fy2026Revenue = LineItem.Create("us-gaap:RevenuesNetOfInterestExpense", "Revenue", 1_218_676_000);
        
        sofiQ2Fy2026Revenue.FlagLineItem();

        sofiQ2Fy2026Revenue.GetEvents().Count.Should().Be(1);
    }
}