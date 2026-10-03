using AwesomeAssertions;
using Tradelens.CompanyFundamentals.Domain.LegalEntities;

namespace Tradelens.CompanyFundamentals.Domain.UnitTests.LegalEntities;

public class LegalEntityTests
{
    [Fact]
    public void LegalEntityTest1()
    {
        var legalEntities = new List<LegalEntity>();
    
        var sofi = LegalEntity.Create("SoFi Technologies, Inc.", new LegalEntityIdentifier("549300SW81JCMVZDDY09"), new CentralIndexKey("0001818874"), new CountryCode("US"), "San Francisco");
        
        var msft = LegalEntity.Create("Microsoft Corporation", new LegalEntityIdentifier("INR2EJN1ERAN0W5ZP974"), new CentralIndexKey("0000789019"), new CountryCode("US"), "Redmond");
    
        legalEntities.AddRange(sofi, msft);
    
        legalEntities.Count.Should().Be(2);
    }
}
