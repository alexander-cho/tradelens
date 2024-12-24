using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreatePostDto
{
    [Required]
    public string Ticker { get; set; }
    [Required]
    public string Content { get; set; }
    public string Sentiment { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}
