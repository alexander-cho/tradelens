using Tradelens.Domain.Entities;

namespace Tradelens.Domain.Specifications;

public class SectorListSpecification : BaseSpecification<Stock, string?>
{
    public SectorListSpecification()
    {
        AddSelect(x => x.Sector);
        ApplyDistinct();
    }
}