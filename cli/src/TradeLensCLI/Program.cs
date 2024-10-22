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
                // break down using helpers if needed
                // while the user doesn't type 'q' to exit the program

                // ask to:
                // add a post, view the whole list, get the posts associated with a specific ticker
                // switch statements
                // if user grabs ticker posts, display and choose one of them?
                // 
                // do validation to see if it is a valid ticker
                PostController postController = new PostController(dbContext);
                postController.GetAllPosts();
                // postController.AddPost();
            }
        }
    }
}
