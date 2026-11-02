namespace BookMyShow.DTOs;

public class CreateShowDto
{
    public int MovieId { get; set; }

    public int ScreensId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal TicketPrice { get; set; }
}
