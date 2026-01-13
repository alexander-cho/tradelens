using FluentAssertions;
using Tradelens.Core.Entities;
using Tradelens.Core.Specifications;

namespace Tradelens.Unit.Tests.Specifications;

/// <summary>
/// Tests for TickerListSpecification rules.
/// </summary>
///
/// <remarks>
/// Currently, the specifications, along with others are "specific", meaning there is only one use case.
/// See if it is possible to make the spec a bit more general, i.e. let it be of any entity type not just of Post,
/// and AddSelect(x => x.[any field of type string]), or even return types other than string.
/// On the other hand, specific specs may be preferable for clear intent
///
/// One thing to note is that the specifications themselves do not execute anything. That is done in the infrastructure
/// layer via the SpecificationEvaluator with IQueryable. This raises the question of what we are trying to test, the
/// intent of the specification, or its execution?
/// </remarks>
public class TickerListSpecificationTests
{
    /// <summary>
    /// This specification is concerned with the unique tickers field given a source type entity of Post.
    /// Use cases elsewhere: The end user wants to filter posts in the feed by ticker symbol.
    /// </summary>
    [Fact]
    public void TickerListSpec_ShouldHave_DistinctEnabled()
    {
        TickerListSpecification tickerListSpecification = new TickerListSpecification();
        bool containsDistinct = tickerListSpecification.IsDistinct;

        containsDistinct.Should().BeTrue();
    }
    
    /// <summary>
    /// Given a list of posts, check that it returns a set of tickers.
    /// This is a borderline integration test and verifies execution.
    /// </summary>
    [Fact]
    public void TickerListSpec_Should_ReturnUniqueTickers()
    {
        var posts = new List<Post>();
        posts.Add(new Post
        {
            Ticker = "JPM",
            Content = "this is about the stock",
            CreatedOn = DateTime.Now,
            Sentiment = "Bullish"
        });
        
        var tickerListSpecification = new TickerListSpecification();
    }
}