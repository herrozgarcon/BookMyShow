namespace BookMyShow.DTOs;

public class BookingCancellationResponseDto
{
    public string Message { get; set; } = "Booking cancelled successfully";
    public int BookingId { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public string Status { get; set; } = "Cancelled";
    public List<ReleasedSeatDto> ReleasedSeats { get; set; } = new();
}

public class ReleasedSeatDto
{
    public int SeatId { get; set; }
    public string Status { get; set; } = "Available";
}
