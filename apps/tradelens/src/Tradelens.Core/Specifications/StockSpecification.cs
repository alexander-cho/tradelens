using Tradelens.Core.Entities;

namespace Tradelens.Core.Specifications;

public class StockSpecification : BaseSpecification<Stock>
{
    public StockSpecification(StockSpecParams stockSpecParams) : base(x =>
        (string.IsNullOrEmpty(stockSpecParams.Search) || x.CompanyName.ToLower().Contains(stockSpecParams.Search)) |
        (string.IsNullOrEmpty(stockSpecParams.Search) || x.Ticker.ToLower().Contains(stockSpecParams.Search)) &&
        (!stockSpecParams.IpoYears.Any() || stockSpecParams.IpoYears.Contains(x.IpoYear)) &&
        (!stockSpecParams.Countries.Any() || (x.Country!= null && stockSpecParams.Countries.Contains(x.Country))) &&
        (!stockSpecParams.Sectors.Any() || (x.Sector!= null && stockSpecParams.Sectors.Contains(x.Sector)))
    )
    {
        switch (stockSpecParams.Sort)
        {
            case "a-z":
                AddOrderBy(x => x.CompanyName);
                break;
            case "z-a":
                AddOrderByDescending(x => x.CompanyName);
                break;
            default:
                AddOrderBy(x => x.Id);
                break;
        }
        
        ApplyPaging((stockSpecParams.PageIndex - 1) * stockSpecParams.PageSize, stockSpecParams.PageSize);
    }
}