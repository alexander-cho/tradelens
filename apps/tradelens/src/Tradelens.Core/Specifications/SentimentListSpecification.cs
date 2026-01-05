using Tradelens.Core.Entities;

namespace Tradelens.Core.Specifications;

public class SentimentListSpecification : BaseSpecification<Post, string>
{
    public SentimentListSpecification()
    {
        AddSelect(x => x.Sentiment);
        ApplyDistinct();
    }
}
