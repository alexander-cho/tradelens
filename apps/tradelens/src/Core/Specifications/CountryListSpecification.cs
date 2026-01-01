using Core.Entities;

namespace Core.Specifications;

public class CountryListSpecification : BaseSpecification<Stock, string?>
{
    public CountryListSpecification()
    {
        AddSelect(x => x.Country);
        ApplyDistinct();
    }
}