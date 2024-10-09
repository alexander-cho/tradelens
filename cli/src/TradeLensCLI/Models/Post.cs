using System.ComponentModel.DataAnnotations;

namespace TradeLensCLI.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Ticker { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Sentiment { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
