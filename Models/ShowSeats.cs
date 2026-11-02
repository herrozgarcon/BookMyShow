using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Models;

public class ShowSeats
{
    [Key]
    public int ShowSeatId { get; set; }

    public int ShowId { get; set; }

    public int SeatId { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Available";

    public Shows Show { get; set; } = null!;

    public Seats Seat { get; set; } = null!;
}
