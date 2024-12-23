using System;
using Core.Entities;

namespace Core.Specifications;

public class PostSpecification : BaseSpecification<Post>
{
    // specify entity, have access to its attributes when defining the specification

    // when PostSpecification class is instantiated, take in the ticker and sentiment as arguments,
    // then pass an expression into the base class (BaseSpecification) with desired queries
    public PostSpecification(string? ticker, string? sentiment, string? sort) : base(x =>
        (string.IsNullOrWhiteSpace(ticker) || x.Ticker == ticker) &&
        (string.IsNullOrWhiteSpace(sentiment) || x.Sentiment == sentiment)
    )
    {
        switch (sort)
        {
            case "earliest":
                AddOrderBy(x => x.CreatedOn);
                break;
            case "latest":
                AddOrderByDescending(x => x.CreatedOn);
                break;
            default:
                AddOrderBy(x => x.Id);
                break;
        }
    }
}
