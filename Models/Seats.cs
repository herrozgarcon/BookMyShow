using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Models;

public class Seats
{
    [Key]
    public int SeatId { get; set; }

    public int ScreensId { get; set; }

    [Required, MaxLength(5)]
    public string RowName { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int SeatNumber { get; set; }

    [Required, MaxLength(30)]
    public string SeatType { get; set; } = string.Empty;

    public Screens Screen { get; set; } = null!;
}
