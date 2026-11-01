using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMyShow.Models;

public class Screens
{
    [Key]
    public int ScreensId { get; set; }

    [ForeignKey(nameof(Theatre))]
    public int TheatreId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string ScreenName { get; set; } = string.Empty;

    //navigation property 
    public Theatres Theatre { get; set; } = null!;
}
