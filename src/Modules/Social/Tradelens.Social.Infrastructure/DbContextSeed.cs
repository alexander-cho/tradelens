using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Infrastructure;

public static class DbContextSeed
{
    /// <summary>
    /// Very quick seeding data for development purposes
    /// </summary>
    /// <param name="dbContext"></param>
    public static async Task SeedPostsAsync(SocialDbContext dbContext)
    {
        if (!dbContext.Posts.Any())
        {
            IList<Post> posts =
            [
                Post.Create(parentPost: null,
                    cashtags:
                    [
                        new Cashtag("NVDA"), new Cashtag("AMD"), new Cashtag("MU"), new Cashtag("AMAT"),
                        new Cashtag("INTC")
                    ],
                    content: "Quite bullish on memory and hardware thanks to demand from AI infrastructure buildout.",
                    sentiment: new Sentiment("Bullish")),
                Post.Create(parentPost: null,
                    cashtags:
                    [
                        new Cashtag("NVDA"), new Cashtag("AMD"), new Cashtag("MU"), new Cashtag("AMAT"),
                        new Cashtag("INTC")
                    ],
                    content: "going to zero going to zero going to zero going to zero going to zero going to zero",
                    sentiment: new Sentiment("Bearish")),
                Post.Create(parentPost: null,
                    cashtags:
                    [
                        new Cashtag("NVDA"), new Cashtag("AMD"), new Cashtag("MU"), new Cashtag("AMAT"),
                        new Cashtag("INTC")
                    ],
                    content: "Quite bullish on memory and hardware thanks to demand from AI infrastructure buildout.",
                    sentiment: new Sentiment("Bullish")),
                Post.Create(parentPost: null,
                    cashtags:
                    [
                        new Cashtag("SOFI")
                    ],
                    content:
                    "SoFi's continued expansion into lending and financial services makes me pretty optimistic about its long-term growth.",
                    sentiment: new Sentiment("Bullish")),
                Post.Create(parentPost: null,
                    cashtags:
                    [
                        new Cashtag("AFRM")
                    ],
                    content:
                    "Buy now pay later is getting way too competitive. Not convinced Affirm can maintain its margins.",
                    sentiment: new Sentiment("Bearish")),
                Post.Create(parentPost: null,
                    cashtags:
                    [
                        new Cashtag("HOOD"), new Cashtag("COIN")
                    ],
                    content:
                    "Retail trading and crypto adoption could be a massive tailwind for both platforms if the next bull cycle takes off.",
                    sentiment: new Sentiment("Bullish")),
                Post.Create(parentPost: null,
                    cashtags:
                    [
                        new Cashtag("SOFI"), new Cashtag("AFRM"), new Cashtag("HOOD"), new Cashtag("COIN")
                    ],
                    content:
                    "Fintech valuations still seem disconnected from the risks. Higher rates and regulatory pressure could make things ugly.",
                    sentiment: new Sentiment("Bearish")),
                Post.Create(parentPost: null,
                    cashtags:
                    [
                        new Cashtag("MSFT"), new Cashtag("AAPL")
                    ],
                    content:
                    "Microsoft's cloud and AI ecosystem is incredibly compelling, while Apple's services business continues to provide a strong foundation.",
                    sentiment: new Sentiment("Bullish")),
                Post.Create(parentPost: null,
                    cashtags:
                    [
                        new Cashtag("AMZN")
                    ],
                    content:
                    "AWS remains a powerhouse, but Amazon's retail margins still leave plenty of room for improvement.",
                    sentiment: new Sentiment("Bullish")),
                Post.Create(parentPost: null,
                    cashtags:
                    [
                        new Cashtag("MSFT"), new Cashtag("AAPL"), new Cashtag("AMZN")
                    ],
                    content:
                    "Big tech feels priced for perfection. Even strong earnings might not be enough to justify these valuations.",
                    sentiment: new Sentiment("Bearish"))
            ];

            await dbContext.AddRangeAsync(posts);

            await dbContext.SaveChangesAsync();
        }
    }
}