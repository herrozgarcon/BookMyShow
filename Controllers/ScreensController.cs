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

    [HttpPost]
    public async Task<IActionResult> AddScreen (CreateScreenDto screenDto)
    {
        await _screenService.AddScreenAsync(screenDto);
        return Ok();
    }
    
    
    
}