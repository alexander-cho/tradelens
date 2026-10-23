using Tradelens.SharedKernel;

namespace Tradelens.Fundamentals.Domain.LegalEntities;

/// <summary>
/// invariants: length must equal 20
/// </summary>
public sealed record LegalEntityIdentifier : ValueObject
{
    public string Value { get; init; }

    private LegalEntityIdentifier(string value)
    {
        Value = value;
        // TODO: invariants: length = 20, etc.
    }

    public static LegalEntityIdentifier Create(string value)
    {
        return new LegalEntityIdentifier(value);
    }
}