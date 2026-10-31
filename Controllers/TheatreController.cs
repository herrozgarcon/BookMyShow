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
    
    
    //adding theatre to DB
    [HttpPost]
    public async Task<IActionResult> CreateTheatre(CreateTheatreDto createTheatreDto)
    {
        
        await _theatreService.AddTheatre(createTheatreDto);
        return Ok("theatre added");
    }

    
    //geting theatres from db
    [HttpGet]
    public async Task<IActionResult> GetAllTheatres()
    {
        var theatreslist = await _theatreService.GetAllTheatres();
        return Ok(theatreslist);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetTheatreById(int id)
    {
        var theatre = await _theatreService.GetTheatrebyID(id);

        if (theatre == null)
        {
            return NotFound("Theatre not found");
        }

        return Ok(theatre);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Updatetheatre(int id,CreateTheatreDto createTheatreDto)
    {

        var theatre = await _theatreService.updateTheatreAsync(id, createTheatreDto);
        if (theatre==null)
        {
            return NotFound("Theatre not found");
        }

        return Ok(theatre);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTheatre(int id)
    {
        var flag=await _theatreService.deleteTheatre(id);
        if (flag)
        {
            return Ok("theatre deleted");
        }
        else
            {
            return NotFound("theatre not found");
            }
        
    }
    
}