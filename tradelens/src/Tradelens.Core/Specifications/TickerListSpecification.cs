using Tradelens.Core.Entities;

namespace Tradelens.Core.Specifications;


// derives from BaseSpecification<T, TResult>, which we additionally defined for return types that are not
// of the same type/entity. In this case the tickers are of type string
public class TickerListSpecification : BaseSpecification<Post, string>
{
    public TickerListSpecification()
    {
        AddSelect(x => x.Ticker);
        ApplyDistinct();
    }
}
