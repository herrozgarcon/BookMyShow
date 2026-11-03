using System.ComponentModel.DataAnnotations;

namespace BookMyShow.DTOs;

public class CreateMovieDto
{
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
    
    
    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Genre { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateOnly ReleaseDate { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Durationminutes { get; set; }
    
    [Required]
    [MaxLength(50)]
    public  string language { get; set; }
    

}