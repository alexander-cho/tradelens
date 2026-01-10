using Tradelens.Core.Constants;

namespace Tradelens.Unit.Tests.Core.Constants;

public class CompanyFundamentalMetricMappingsTests
{
    [Fact]
    public void TestMappingKeysContainThreeTypes()
    {
        var mapping = CompanyFundamentalMetricMappings.MetricToEndpoint;
    }
}
