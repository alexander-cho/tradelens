using System.ComponentModel.DataAnnotations;

namespace Tradelens.Api.DTOs;

public class RegisterDto
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    [Required] 
    public string Email { get; set; } = string.Empty;
    
    [Required] 
    public string Password { get; set; } = string.Empty;
}
