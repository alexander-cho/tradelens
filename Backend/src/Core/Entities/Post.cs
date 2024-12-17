using System;

namespace Core.Entities;

// enum Sentiment { Bullish, Bearish };

public class Post
{
    public int Id { get; set; }
    public string? Ticker { get; set; }
    public string? Content { get; set; }
    // Sentiment
    // CreatedOn timestamp

}
