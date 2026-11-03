using BookMyShow.DTOs;
using BookMyShow.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookMyShow.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class SeatsController : ControllerBase
{
    private readonly ISeatsService _service;
    public SeatsController(ISeatsService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSeatDto dto)
    {
        try { var seat = await _service.AddAsync(dto); return CreatedAtAction(nameof(GetById), new { id = seat.SeatId }, seat); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> CreateBulk([FromBody] BulkCreateSeatsDto dto)
    {
        try { return Ok(await _service.AddBulkAsync(dto)); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }

    [HttpGet("screen/{screenId:int}")]
    public async Task<IActionResult> GetByScreen(int screenId)
    {
        try { return Ok(await _service.GetByScreenAsync(screenId)); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try { return Ok(await _service.GetByIdAsync(id)); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSeatDto dto)
    {
        try { return Ok(await _service.UpdateAsync(id, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _service.DeleteAsync(id); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }
}
