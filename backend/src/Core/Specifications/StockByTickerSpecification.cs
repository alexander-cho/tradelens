using Core.Entities;

namespace Core.Specifications;

public class StockByTickerSpecification : BaseSpecification<Stock>
{
    public StockByTickerSpecification(string ticker)
        : base(x => x.Ticker == ticker)
    {
    }
}