using System.ComponentModel.DataAnnotations;

namespace BookMyShow.DTOs;

public class CreateScreenDto
{
    
    [Required]
    public string ScreenName { get; set; } = string.Empty;
    
    [Required]
    public int TheatreId { get; set; }
    
    
}