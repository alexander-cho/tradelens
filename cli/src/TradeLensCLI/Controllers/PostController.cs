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

        public void GetAllPosts()
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

        public void GetPost()
        {
            using (dbContext)
            {
                // getting one post logic, retrieve by (id)?
            }
        }

        public void GetTickerPosts()
        {
            using (dbContext)
            {
                // logic to retrieve all posts associated with a particular ticker
                // prompt user for a valid ticker
            }
        }

        public void AddPost()
        {
            using (dbContext)
            {
                // adding one post logic
                // get user input for each field.
                Post post = new Post {Ticker="SOFI", Body="Great company.", Sentiment="Bullish"};
                dbContext.Add(post);
                dbContext.SaveChanges();
            }
        }

        public void UpdatePost()
        {
            using (dbContext)
            {
                // updating one post logic
            }
        }

        public void DeletePost()
        {
            using (dbContext)
            {
                // deleting one post logic
            }
        }
    }
}
