using Tradelens.SharedKernel;

namespace Tradelens.CompanyFundamentals.Domain.LegalEntities;

public sealed record CountryCode : ValueObject
{
    public string Value { get; private set; }

    private CountryCode(string value)
    {
        Value = value;
    }

    public static CountryCode Create(string value)
    {
        // TODO: invariants: length = 2, must be uppercase, etc.
        return new CountryCode(value);
    }
}