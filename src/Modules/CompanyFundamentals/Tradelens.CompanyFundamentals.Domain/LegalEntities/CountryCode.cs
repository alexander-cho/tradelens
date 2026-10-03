namespace Tradelens.CompanyFundamentals.Domain.LegalEntities;

// public class CountryCode
// {
//     
// }

/// <summary>
/// invariants:
/// length must equal 2.
/// </summary>
/// <param name="Value"></param>
public record CountryCode(
    string Value
);