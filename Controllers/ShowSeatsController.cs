using BookMyShow.DTOs;
using BookMyShow.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowSeatsController : ControllerBase
{
    private readonly IShowSeatsService _showSeatsService;

    public ShowSeatsController(IShowSeatsService showSeatsService) => _showSeatsService = showSeatsService;

    [HttpGet]
    public async Task<IActionResult> GetByShow([FromQuery] int showId)
    {
        try
        {
            var showSeats = await _showSeatsService.GetByShowAsync(showId);
            return Ok(ToResponse(showId, showSeats));
        }
        catch (KeyNotFoundException) { return NotFound("Show not found."); }
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable([FromQuery] int showId)
    {
        try
        {
            var showSeats = await _showSeatsService.GetAvailableByShowAsync(showId);
            return Ok(ToResponse(showId, showSeats));
        }
        catch (KeyNotFoundException) { return NotFound("Show not found."); }
    }

    [HttpGet("booked")]
    public async Task<IActionResult> GetBooked([FromQuery] int showId)
    {
        try
        {
            var showSeats = await _showSeatsService.GetBookedByShowAsync(showId);
            return Ok(ToResponse(showId, showSeats));
        }
        catch (KeyNotFoundException) { return NotFound("Show not found."); }
    }

    private static ShowSeatsResponseDto ToResponse(int showId, IEnumerable<BookMyShow.Models.ShowSeats> showSeats) => new()
    {
        ShowId = showId,
        Seats = showSeats.Select(showSeat => new ShowSeatDto
        {
            ShowSeatId = showSeat.ShowSeatId,
            SeatId = showSeat.SeatId,
            RowName = showSeat.Seat.RowName,
            SeatNumber = showSeat.Seat.SeatNumber,
            SeatType = showSeat.Seat.SeatType,
            Status = showSeat.Status
        }).ToList()
    };
}
