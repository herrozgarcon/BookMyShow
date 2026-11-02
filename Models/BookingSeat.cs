using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Models;

public class BookingSeat
{
    [Key] public int BookingSeatId { get; set; }
    public int BookingId { get; set; }
    public int SeatId { get; set; }
    public decimal Price { get; set; }
    public Booking Booking { get; set; } = null!;
    public Seats Seat { get; set; } = null!;
}
