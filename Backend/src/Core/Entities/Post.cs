using System;

namespace Core.Entities;

// enum Sentiment { Bullish, Bearish };

public class Post : BaseEntity
{
    public required string Ticker { get; set; }
    public required string Content { get; set; }
    public required string Sentiment { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;

}
