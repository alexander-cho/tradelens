using Tradelens.Core.Entities;

namespace Tradelens.Core.Specifications;

public class IpoYearListSpecification : BaseSpecification<Stock, int?>
{
    public IpoYearListSpecification()
    {
        AddSelect(x => x.IpoYear);
        ApplyDistinct();
    }
}