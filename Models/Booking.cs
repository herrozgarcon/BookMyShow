using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Models;

public class Booking
{
    [Key] public int BookingId { get; set; }
    [Required, MaxLength(30)] public string BookingReference { get; set; } = string.Empty;
    public int ShowId { get; set; }
    [Required, MaxLength(100)] public string CustomerName { get; set; } = string.Empty;
    [Required, MaxLength(150)] public string CustomerEmail { get; set; } = string.Empty;
    [Required, MaxLength(15)] public string CustomerPhone { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public decimal TotalAmount { get; set; }
    [Required, MaxLength(20)] public string Status { get; set; } = "Confirmed";
    public Shows Show { get; set; } = null!;
    public ICollection<BookingSeat> Seats { get; set; } = new List<BookingSeat>();
}
