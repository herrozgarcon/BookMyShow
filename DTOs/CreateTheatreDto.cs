using System.ComponentModel.DataAnnotations;

namespace BookMyShow.DTOs;

public class CreateTheatreDto
{
    
    [Required]
    [StringLength(50,MinimumLength = 2)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(150,MinimumLength = 2)]
    public string Location { get; set; }
}