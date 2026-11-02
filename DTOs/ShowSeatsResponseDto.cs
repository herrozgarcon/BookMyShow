namespace BookMyShow.DTOs;

public class ShowSeatsResponseDto
{
    public int ShowId { get; set; }

    public List<ShowSeatDto> Seats { get; set; } = new();
}
