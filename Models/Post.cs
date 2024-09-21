using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeLens.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public string Body { get; set; }
        public string Sentiment { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}