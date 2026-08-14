using Tradelens.Domain.Entities;

namespace Tradelens.Domain.Specifications;

public class CountryListSpecification : BaseSpecification<Stock, string?>
{
    public CountryListSpecification()
    {
        AddSelect(x => x.Country);
        ApplyDistinct();
    }
}