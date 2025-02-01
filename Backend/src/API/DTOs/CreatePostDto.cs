using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

// https://stackoverflow.com/questions/43688968/what-does-it-mean-for-a-property-to-be-required-and-nullable

public class CreatePostDto
{
    [Required]
    public string? Ticker { get; set; }
    [Required]
    public string? Content { get; set; }
    public string? Sentiment { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}
