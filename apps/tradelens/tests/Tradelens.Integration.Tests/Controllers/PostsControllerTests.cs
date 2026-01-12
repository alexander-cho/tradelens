using System.Net.Http.Json;
using AutoFixture;
using Tradelens.Core.Entities;
using FluentAssertions;
using Tradelens.Infrastructure.Data;

namespace Tradelens.Integration.Tests.Controllers;

public class PostsControllerTests : IClassFixture<TradelensWebApplicationFactory>
{
    private readonly Fixture _fixture = new();
    private readonly HttpClient _client;
    private readonly TradelensDbContext _dbContext;

    public PostsControllerTests(TradelensWebApplicationFactory factory)
    {
        _dbContext = factory.Db;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task WhenPostExists_ThenReturnIt()
    {
        var existingPost = _fixture.Create<Post>();
        // override AutoFixture default behavior of creating DateTime fields
        existingPost.CreatedOn = DateTime.SpecifyKind(existingPost.CreatedOn, DateTimeKind.Utc);
        await _dbContext.AddAsync(existingPost);
        await _dbContext.SaveChangesAsync();

        var post = await _client.GetFromJsonAsync<Post>($"https://localhost:6001/api/Posts/{existingPost.Id}");

        post.Should().NotBeNull();
    }
}