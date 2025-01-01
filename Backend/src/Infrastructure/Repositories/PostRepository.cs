using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PostRepository(TradeLensDbContext context) : IPostRepository
{
    public async Task<IReadOnlyList<Post>> GetPostsAsync(string? ticker, string? sentiment, string? sort)
    {
        // build up query to pass to EF with params
        var query = context.Posts.AsQueryable();

        // query params are optional, so check if they are there
        if (!string.IsNullOrWhiteSpace(ticker))
        {
            query = query.Where(x => x.Ticker == ticker);
        }

        if (!string.IsNullOrWhiteSpace(sentiment))
        {
            query = query.Where(x => x.Sentiment == sentiment);
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            query = sort switch
            {
                "earliest" => query.OrderBy(x => x.CreatedOn),
                "latest" => query.OrderByDescending(x => x.CreatedOn),
                _ => query.OrderBy(x => x.Id)
            };
        }

        // return await context.Posts.ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await context.Posts.FindAsync(id);
    }

    public void AddPost(Post post)
    {
        context.Posts.Add(post);
    }

    public void UpdatePost(Post post)
    {
        context.Entry(post).State = EntityState.Modified;
    }

    public void DeletePost(Post post)
    {
        context.Posts.Remove(post);
    }

    public bool PostExists(int id)
    {
        return context.Posts.Any(x => x.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<IReadOnlyList<string>> GetTickers()
    {
        return await context.Posts
            .Select(x => x.Ticker)
            .Distinct()
            .ToListAsync();
    }
}
