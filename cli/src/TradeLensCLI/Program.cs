using System;
using System.Linq;
using TradeLensCLI.DbContexts;
using TradeLensCLI.Controllers;
using Microsoft.EntityFrameworkCore;

namespace TradeLensCLI
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (CliDbContext dbContext = new CliDbContext())
            {
                Console.WriteLine("Hello");
                PostController postController = new PostController(dbContext);
                postController.LoadAllPosts();
            }
        }
    }
}
