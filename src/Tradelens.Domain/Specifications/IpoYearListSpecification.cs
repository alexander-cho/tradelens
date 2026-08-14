using Tradelens.Domain.Entities;

namespace Tradelens.Domain.Specifications;

public class IpoYearListSpecification : BaseSpecification<Stock, int?>
{
    public IpoYearListSpecification()
    {
        AddSelect(x => x.IpoYear);
        ApplyDistinct();
    }
}