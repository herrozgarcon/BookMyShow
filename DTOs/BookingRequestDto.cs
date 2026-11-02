using System.ComponentModel.DataAnnotations;

namespace BookMyShow.DTOs;

public class BookingRequestDto
{
    [Range(1, int.MaxValue)] public int ShowId { get; set; }
    [Required, StringLength(100), MinLength(1)] public string CustomerName { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(150)] public string CustomerEmail { get; set; } = string.Empty;
    [Required, Phone, StringLength(15, MinimumLength = 7)] public string CustomerPhone { get; set; } = string.Empty;
    [Required, MinLength(1)] public List<int> SeatIds { get; set; } = new();
}
