namespace Tradelens.CompanyFundamentals.Domain.LegalEntities;

public readonly record struct LegalEntityId(Guid Value);

/// <summary>
/// Represents the central anchor for a particular legal/business entity, e.g. "MICROSOFT CORPORATION",
/// "SOFI TECHNOLOGIES, INC.", etc. A majority have a globally unique 20-digit Legal Entity Identifier (LEI).
/// Those filing their business operations and results with the SEC also have a 10-digit Central Index Key (CIK).
/// The legal name of a legal entity can change as well, in recent years notably, Facebook -> Meta.
/// They can also move their incorporated headquarters city
/// </summary>
public class LegalEntity
{
    public LegalEntityId Id { get; init; }
    public string LegalName { get; private set; }
    public LegalEntityIdentifier? LegalEntityIdentifier { get; }
    public CentralIndexKey? CentralIndexKey { get; }
    public CountryCode CountryCode { get; }
    public string HeadquartersCity { get; private set; }

    private List<object> _events = new List<object>();

    private LegalEntity(string legalName, LegalEntityIdentifier? legalEntityIdentifier,
        CentralIndexKey? centralIndexKey,
        CountryCode countryCode, string headquartersCity)
    {
        Id = new LegalEntityId(Guid.CreateVersion7());
        LegalName = legalName;
        LegalEntityIdentifier = legalEntityIdentifier;
        CentralIndexKey = centralIndexKey;
        CountryCode = countryCode;
        HeadquartersCity = headquartersCity;
    }

    public static LegalEntity Create(string legalName, LegalEntityIdentifier? legalEntityIdentifier,
        CentralIndexKey? centralIndexKey,
        CountryCode countryCode, string headquartersCity)
    {
        return new LegalEntity(legalName, legalEntityIdentifier, centralIndexKey, countryCode,
            headquartersCity);
    }
}

public record LegalEntityAdded(Guid LegalEntityId, DateTime OccuredAt);

public record LegalNameChanged(Guid LegalEntityId, DateTime OccuredAt);