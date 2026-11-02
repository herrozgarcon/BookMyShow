using BookMyShow.DTOs;
using BookMyShow.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookMyShow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowsController : ControllerBase
{
    private readonly IShowsService _showsService;

    public ShowsController(IShowsService showsService) => _showsService = showsService;

    [Authorize(Roles = "Admin"), HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShowDto dto)
    {
        try
        {
            var show = await _showsService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = show.ShowId }, show);
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _showsService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var show = await _showsService.GetByIdAsync(id);
        return show == null ? NotFound("Show not found.") : Ok(show);
    }

    [HttpGet("movie/{movieId:int}")]
    public async Task<IActionResult> GetByMovie(int movieId) => Ok(await _showsService.GetByMovieAsync(movieId));

    [HttpGet("theatre/{theatreId:int}")]
    public async Task<IActionResult> GetByTheatre(int theatreId) => Ok(await _showsService.GetByTheatreAsync(theatreId));

    [HttpGet("search")]
    public async Task<IActionResult> SearchByDate([FromQuery] DateOnly date) => Ok(await _showsService.SearchByDateAsync(date));

    [Authorize(Roles = "Admin"), HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateShowDto dto)
    {
        try
        {
            var show = await _showsService.UpdateAsync(id, dto);
            return show == null ? NotFound("Show not found.") : Ok(show);
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }

    [Authorize(Roles = "Admin"), HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _showsService.DeleteAsync(id))
            return NotFound("Show not found.");

        return NoContent();
    }
}
