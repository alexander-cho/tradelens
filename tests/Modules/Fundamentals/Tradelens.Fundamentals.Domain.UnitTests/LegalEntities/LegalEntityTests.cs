using AwesomeAssertions;
using Tradelens.Fundamentals.Domain.LegalEntities;

namespace Tradelens.Fundamentals.Domain.UnitTests.LegalEntities;

public class LegalEntityTests
{
    [Fact]
    public void LegalEntityTest1()
    {
        var legalEntities = new List<LegalEntity>();

        var sofi = LegalEntity.Create("SoFi Technologies, Inc.", LegalEntityIdentifier.Create("549300SW81JCMVZDDY09"),
            CentralIndexKey.Create("0001818874"), CountryCode.Create("US"), "San Francisco");

        var msft = LegalEntity.Create("Microsoft Corporation", LegalEntityIdentifier.Create("INR2EJN1ERAN0W5ZP974"),
            CentralIndexKey.Create("0000789019"), CountryCode.Create("US"), "Redmond");

        legalEntities.AddRange(sofi, msft);

        legalEntities.Count.Should().Be(2);
    }
}