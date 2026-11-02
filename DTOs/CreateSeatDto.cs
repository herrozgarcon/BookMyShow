using System.ComponentModel.DataAnnotations;

namespace BookMyShow.DTOs;

public class CreateSeatDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ScreensId { get; set; }

    [Required, StringLength(5, MinimumLength = 1)]
    public string RowName { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int SeatNumber { get; set; }

    [Required, StringLength(30, MinimumLength = 1)]
    public string SeatType { get; set; } = string.Empty;
}
