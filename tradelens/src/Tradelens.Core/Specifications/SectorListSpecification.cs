using Tradelens.Core.Entities;

namespace Tradelens.Core.Specifications;

public class SectorListSpecification : BaseSpecification<Stock, string?>
{
    public SectorListSpecification()
    {
        AddSelect(x => x.Sector);
        ApplyDistinct();
    }
}