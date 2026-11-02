using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Models;

public class Shows
{
    [Key]
    public int ShowId { get; set; }

    public int MovieId { get; set; }

    public int ScreensId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal TicketPrice { get; set; }

    public Movies Movie { get; set; } = null!;

    public Screens Screen { get; set; } = null!;
}
