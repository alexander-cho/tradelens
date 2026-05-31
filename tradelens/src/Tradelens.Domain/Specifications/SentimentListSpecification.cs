using Tradelens.Domain.Entities;

namespace Tradelens.Domain.Specifications;

public class SentimentListSpecification : BaseSpecification<Post, string>
{
    public SentimentListSpecification()
    {
        AddSelect(x => x.Sentiment);
        ApplyDistinct();
    }
}
