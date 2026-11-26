using AwesomeAssertions;
using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Domain.UnitTests.Posts;

public class PostTests
{
    [Fact]
    public void GivenOverFiveCashtags_WhenPostCreateAttempt_ThenCreationFails()
    {
        var post = Post.Create(
            parentPostId: null,
            cashtags:
            [
                new Cashtag("NVDA"), new Cashtag("AMD"), new Cashtag("MU"), new Cashtag("AMAT"), new Cashtag("INTC"),
                new Cashtag("ARM")
            ],
            content: "Quite bullish on memory and hardware thanks to demand from AI infrastructure buildout.",
            sentiment: new Sentiment("Bullish")
        );

        post.IsSuccess.Should().BeFalse();
        post.GetError().Should().Be(PostErrors.MaxCashtagsPerPostLimitExceeded);
    }

    [Fact]
    public void GivenPostContentLessThanLength50_WhenPostCreateAttempt_ThenCreationFails()
    {
        var post = Post.Create(
            parentPostId: null,
            cashtags:
            [
                new Cashtag("NVDA"), new Cashtag("AMD"), new Cashtag("MU"), new Cashtag("AMAT"), new Cashtag("INTC")
            ],
            content: "going to zero",
            sentiment: new Sentiment("Bearish")
        );

        post.IsSuccess.Should().BeFalse();
        post.GetError().Should().Be(PostErrors.MinimumContentLengthNotMet);
    }

    [Fact]
    public void GivenPostContentEmpty_WhenPostCreateAttempt_ThenCreationFails()
    {
        var post = Post.Create(
            parentPostId: null,
            cashtags:
            [
                new Cashtag("SOFI"), new Cashtag("HOOD")
            ],
            content: "",
            sentiment: new Sentiment("Bullish")
        );

        post.IsSuccess.Should().BeFalse();
        post.GetError().Should().Be(PostErrors.EmptyContent);
    }

    [Fact]
    public void GivenValidPost_WhenPostCreateAttempt_ThenCreationSucceeds()
    {
        var post = Post.Create(
            parentPostId: null,
            cashtags:
            [
                new Cashtag("SOFI")
            ],
            content: "Continued expansion into lending makes me optimistic about its long-term growth.",
            sentiment: new Sentiment("Bullish")
        );

        post.IsSuccess.Should().BeTrue();
    }

    // [Fact]
    // public void GivenOver5MinutesPassedSincePostCreated_WhenPostDeleteAttempt_ThenDeletionFails()
    // {
    //     var post = Post.Create(
    //         parentPostId: null,
    //         cashtags:
    //         [
    //             new Cashtag("NVDA"), new Cashtag("AMD"), new Cashtag("MU"), new Cashtag("AMAT"), new Cashtag("INTC")
    //         ],
    //         content: "Quite bullish on memory and hardware thanks to demand from AI infrastructure buildout.",
    //         sentiment: new Sentiment("Bullish")
    //     );
    // }
}