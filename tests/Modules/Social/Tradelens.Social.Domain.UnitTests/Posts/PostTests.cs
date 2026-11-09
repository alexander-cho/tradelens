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
                new Cashtag("NVDA"), new Cashtag("AMD"), new Cashtag("MU"), new Cashtag("AMAT"), new Cashtag("INTC"),
                new Cashtag("ARM")
            ],
            content: "Quite bullish on memory and hardware thanks to demand from AI infrastructure buildout.",
            sentiment: new Sentiment("Bullish")
        );
    }

    [Fact]
    public void GivenPostContentLessThanLength50_WhenPostCreateAttempt_ThenCreationFails()
    {
        var post = Post.Create(
            parentPost: null,
            cashtags:
            [
                new Cashtag("NVDA"), new Cashtag("AMD"), new Cashtag("MU"), new Cashtag("AMAT"), new Cashtag("INTC")
            ],
            content: "going to zero",
            sentiment: new Sentiment("Bearish")
        );
    }

    [Fact]
    public void GivenOver5MinutesPassedSincePostCreated_WhenPostDeleteAttempt_ThenDeletionFails()
    {
        var post = Post.Create(
            parentPost: null,
            cashtags:
            [
                new Cashtag("NVDA"), new Cashtag("AMD"), new Cashtag("MU"), new Cashtag("AMAT"), new Cashtag("INTC")
            ],
            content: "Quite bullish on memory and hardware thanks to demand from AI infrastructure buildout.",
            sentiment: new Sentiment("Bullish")
        );
    }
}