using Tradelens.SharedKernel;

namespace Tradelens.Fundamentals.Domain.LegalEntities;

/// <summary>
/// invariants: length must equal 10
/// many providers or platforms use CIKs without the left padded zeroes, so always pad if the length is less than 10
/// </summary>
public sealed record CentralIndexKey : ValueObject
{
    public string Value { get; private set; }

    private CentralIndexKey(string value)
    {
        Value = value;
        // TODO: invariants: length = 10, left padding with zeros to fit 10 digits, etc.
    }

    public static CentralIndexKey Create(string value)
    {
        return new CentralIndexKey(value);
    }
}