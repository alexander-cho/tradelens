using Tradelens.Core.Entities;

namespace Tradelens.Core.Specifications;

public class CountryListSpecification : BaseSpecification<Stock, string?>
{
    public CountryListSpecification()
    {
        AddSelect(x => x.Country);
        ApplyDistinct();
    }
}