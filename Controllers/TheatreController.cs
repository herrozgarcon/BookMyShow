using BookMyShow.DTOs;
using BookMyShow.Models;
using BookMyShow.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TheatreController : Controller
{

    private readonly ITheatreService _theatreService;

    public TheatreController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTheatre(CreateTheatreDto createTheatreDto)
    {
        
        await _theatreService.AddTheatre(createTheatreDto);
        return Ok("theatre added");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTheatres()
    {
        var theatreslist = await _theatreService.GetAllTheatres();
        return Ok(theatreslist);
    }
    
    
}