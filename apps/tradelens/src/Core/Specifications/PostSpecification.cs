using Core.Entities;

namespace Core.Specifications;

public class PostSpecification : BaseSpecification<Post>
{
    // specify entity, have access to its attributes when defining the specification

    // when PostSpecification class is instantiated, take in the ticker and sentiment as arguments,
    // then pass an expression into the base class (BaseSpecification) with desired queries

    // instead of listing params one by one, PostSpecParams allows us to use a single object
    public PostSpecification(PostSpecParams postSpecParams) : base(x =>
        (string.IsNullOrEmpty(postSpecParams.Search) || x.Content.ToLower().Contains(postSpecParams.Search)) &&
        (!postSpecParams.Tickers.Any() || postSpecParams.Tickers.Contains(x.Ticker)) &&
        (!postSpecParams.Sentiments.Any() || postSpecParams.Sentiments.Contains(x.Sentiment))
    )
    {
        switch (postSpecParams.Sort)
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

        // pagination: i.e. if page index is 2 (second page) and page size is 5; take 5, skip 5
        ApplyPaging((postSpecParams.PageIndex - 1) * postSpecParams.PageSize, postSpecParams.PageSize);
    }
}
