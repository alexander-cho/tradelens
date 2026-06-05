using Tradelens.Discussion.Domain.PostAggregate;

namespace Tradelens.Discussion.Domain.UnitTests.PostAggregate;

public class PostTests
{
    [Fact]
    public void PostTest1()
    {
        var post = Post.Create(userId: 1, text: "hello this is the first post from Alex $IWM");
    }
}