namespace Tradelens.CompanyFundamentals.Domain.LegalEntities;

// public class CentralIndexKey
// {
//     
// }

/// <summary>
/// invariants: length must equal 10
/// many providers or platforms use CIK's without the left padded zeroes, so always pad if the length is less than 10
/// </summary>
/// <param name="Cik"></param>
public class CentralIndexKey(
    string Cik
);