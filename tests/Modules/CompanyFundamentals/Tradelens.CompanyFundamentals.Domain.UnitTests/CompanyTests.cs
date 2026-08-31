using AwesomeAssertions;

namespace Tradelens.CompanyFundamentals.Domain.UnitTests;

public class CompanyTests
{
    [Fact]
    public void CompanyTest1()
    {
        var companyStore = new List<Company>();
            
        var sofi = new Company
        {
            LegalName = "SoFi Technologies, Inc.",
            LegalEntityIdentifier = "549300SW81JCMVZDDY09",
            CountryOfIncorporation = new CountryOfIncorporation("US"),
            HeadquartersCity = new HeadquartersCity("San Francisco")
        };
        
        var msft = new Company
        {
            LegalName = "Microsoft Corporation, Inc.",
            LegalEntityIdentifier = "INR2EJN1ERAN0W5ZP974",
            CountryOfIncorporation = new CountryOfIncorporation("US"),
            HeadquartersCity = new HeadquartersCity("Redmond")
        };

        companyStore.AddRange(sofi, msft);

        companyStore.Count.Should().Be(2);
    }
}
