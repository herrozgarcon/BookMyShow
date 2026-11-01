using BookMyShow.DTOs;
using BookMyShow.Models;
using BookMyShow.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ScreensController : Controller
{
    
    private readonly IScreenService _screenService;
    public ScreensController(IScreenService screenService)
    {
        _screenService = screenService;
    }
    //Add screen
    [HttpPost]
    public async Task<IActionResult> AddScreen (CreateScreenDto screenDto)
    {
        await _screenService.AddScreenAsync(screenDto);
        return Ok();
    }
    
    // GET ALL SCREENS
    [HttpGet]
    public async Task<IActionResult> GetAllScreens()
    {
        var screens = await _screenService.GetAllScreensAsync();
        return Ok(screens);
    }

    // GET SCREEN BY ID
    [HttpGet("search")]
    public async Task<IActionResult> GetScreenById([FromQuery] int id)
    {
        var screen = await _screenService.GetScreenByIdAsync(id);

        if (screen == null)
            return NotFound("Screen not found.");

        return Ok(screen);
    }

    // UPDATE SCREEN
    [HttpPut("update")]
    public async Task<IActionResult> UpdateScreen(
        [FromQuery] int id,
        [FromBody] CreateScreenDto createScreenDto)
    {
        try
        {
            await _screenService.UpdateScreenAsync(id, createScreenDto);
            return Ok("Screen updated successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // DELETE SCREEN
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteScreen([FromQuery] int id)
    {

        if (await _screenService.DeleteScreenAsync(id))
        {
            return Ok("Screen deleted successfully.");
        }
    
            return NotFound("Screen not found.");
        
    }

    
    
    
    
}