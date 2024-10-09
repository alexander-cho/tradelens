using System.Linq;
using Microsoft.EntityFrameworkCore;
using TradeLensCLI.DbContexts;
using TradeLensCLI.Models;

namespace TradeLensCLI.Controllers
{
    public class PostController
    {
        private readonly DbContext dbContext;
        
        public PostController(DbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void LoadAllPosts()
        {
            using (dbContext)
            {
                var allPosts = dbContext.Set<Post>().ToList();
                foreach (var post in allPosts)
                {
                    Console.WriteLine($"Post ID: {post.Id}, Ticker: {post.Ticker}, Body: {post.Body}, Sentiment: {post.Sentiment}");
                }
            }
        }

        public void AddPost()
        {
            using (dbContext)
            {
                // adding one post logic
            }
        }
    }
}