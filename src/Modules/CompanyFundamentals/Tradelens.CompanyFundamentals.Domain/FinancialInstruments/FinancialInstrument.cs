using Tradelens.CompanyFundamentals.Domain.LegalEntities;

namespace Tradelens.CompanyFundamentals.Domain.FinancialInstruments;

public class FinancialInstrument
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public LegalEntity? LegalEntity { get; private set; }
    // Isin, Cusip, Figi -> collection of value object Identifier.cs
    // enum for type of instrument like stock, bond, ETF, etc.
}