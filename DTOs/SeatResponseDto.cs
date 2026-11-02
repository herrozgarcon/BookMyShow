namespace BookMyShow.DTOs;

public class SeatResponseDto
{
    public int SeatId { get; set; }
    public int ScreensId { get; set; }
    public string RowName { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public string SeatType { get; set; } = string.Empty;
}
