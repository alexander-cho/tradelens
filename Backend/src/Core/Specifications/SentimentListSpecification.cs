using System;
using Core.Entities;

namespace Core.Specifications;

public class SentimentListSpecification : BaseSpecification<Post, string>
{
    public SentimentListSpecification()
    {
        AddSelect(x => x.Sentiment);
        ApplyDistinct();
    }
}
