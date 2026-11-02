using BookMyShow.DTOs;
using BookMyShow.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _service;
    public BookingsController(IBookingService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _service.CreateAsync(request, cancellationToken);
            return booking == null ? NotFound("Show not found.") : CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, booking);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel([FromQuery] int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.CancelAsync(id, cancellationToken);
            return result == null ? NotFound("Booking not found.") : Ok(result);
        }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }

    [HttpGet] public async Task<IActionResult> GetAll(CancellationToken ct) => Ok(await _service.GetAllAsync(ct));

    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] int id, CancellationToken ct)
    {
        var booking = await _service.GetByIdAsync(id, ct); return booking == null ? NotFound("Booking not found.") : Ok(booking);
    }

    [HttpGet("reference/{reference}")]
    public async Task<IActionResult> GetByReference(string reference, CancellationToken ct)
    {
        var booking = await _service.GetByReferenceAsync(reference, ct); return booking == null ? NotFound("Booking not found.") : Ok(booking);
    }
    
    
    [HttpGet("show")] public async Task<IActionResult> GetByShow([FromQuery] int showId, CancellationToken ct) => Ok(await _service.GetByShowAsync(showId, ct));
}
