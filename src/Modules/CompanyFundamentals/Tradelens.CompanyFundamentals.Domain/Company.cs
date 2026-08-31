namespace Tradelens.CompanyFundamentals.Domain;

public class Company
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string LegalName { get; set; }
    public required string LegalEntityIdentifier { get; init; }
    public double? CentralIndexKey { get; init; }
    public CountryOfIncorporation CountryOfIncorporation { get; set; }
    public HeadquartersCity HeadquartersCity { get; set; }
    
    private List<object> _events = new List<object>();
    
    
}

public struct CountryOfIncorporation(
    string CountryCode
);

public struct HeadquartersCity(
    string City
);

public record CompanyAdded(Guid CompanyId, DateTime AddedAt);
public record LegalNameChanged(Guid CompanyId, DateTime ChangedAt);