using System.Net.Http.Json;
using Core.Entities;
using FluentAssertions;

namespace API.Tests;

public class PostsControllerTests
{
    [Fact]
    public async Task TestValidPostCreation()
    {
        // Arrange
        var application = new TradelensWebApplicationFactory();
        Post post = new Post
        {
            Ticker = "SOFI",
            Content = "A great stock!",
            Sentiment = "Bullish"
        };

        var client = application.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/posts", post);

        //Assert
        response.EnsureSuccessStatusCode();
        var postResponse = await response.Content.ReadFromJsonAsync<Post>();
        postResponse?.Id.Should().BePositive();
    }
}