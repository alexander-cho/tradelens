using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Domain.UnitTests.Posts;

public class PostTests
{
    [Fact]
    public void GivenOverFiveCashtags_WhenPostCreateAttempt_ThenCreationFails()
    {
        var post = Post.Create(
            parentPost: null,
            cashtags:
            [
                Cashtag.Create("NVDA"), Cashtag.Create("AMD"), Cashtag.Create("MU"), Cashtag.Create("AMAT"),
                Cashtag.Create("INTC"), Cashtag.Create("ARM")
            ],
            content: "Quite bullish on memory and hardware thanks to demand from AI infrastructure buildout.",
            sentiment: Sentiment.Bullish
        );
    }

    [Fact]
    public void GivenPostContentLessThanLength50_WhenPostCreateAttempt_ThenCreationFails()
    {
        var post = Post.Create(
            parentPost: null,
            cashtags:
            [
                Cashtag.Create("NVDA"), Cashtag.Create("AMD"), Cashtag.Create("MU"), Cashtag.Create("AMAT"),
                Cashtag.Create("INTC")
            ],
            content: "going to zero",
            sentiment: Sentiment.Bearish
        );
    }

    [Fact]
    public void GivenOver5MinutesPassedSincePostCreated_WhenPostDeleteAttempt_ThenDeletionFails()
    {
        var post = Post.Create(
            parentPost: null,
            cashtags:
            [
                Cashtag.Create("NVDA"), Cashtag.Create("AMD"), Cashtag.Create("MU"), Cashtag.Create("AMAT"),
                Cashtag.Create("INTC")
            ],
            content: "Quite bullish on memory and hardware thanks to demand from AI infrastructure buildout.",
            sentiment: Sentiment.Bullish
        );
    }
}