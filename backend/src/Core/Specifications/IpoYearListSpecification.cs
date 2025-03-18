using Core.Entities;

namespace Core.Specifications;

public class IpoYearListSpecification : BaseSpecification<Stock, int?>
{
    public IpoYearListSpecification()
    {
        AddSelect(x => x.IpoYear);
        ApplyDistinct();
    }
}