using BookMyShow.DTOs;
using BookMyShow.Migrations;
using BookMyShow.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookMyShow.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MoviesController : Controller
{
    
    private readonly IMoviesService _moviesService;

    public MoviesController(IMoviesService moviesService)
    {
        _moviesService = moviesService;
    }
    
    [Authorize(Roles = "Admin"), HttpPost]
    public async  Task<ActionResult> addMovie(CreateMovieDto createMovieDto)
    {
        await _moviesService.AddMovieAsync(createMovieDto);
        return Ok("Movie added");
    }

    [HttpGet]
    public async Task<ActionResult<List<Movies>>> GetAllMovies()
    {
        var moviedto=await _moviesService.GetAllMoviesAsync();
        return Ok(moviedto);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Movies?>> GetMovieById(int id)
    {
        var moviedto = await _moviesService.GetMovieByIdAsync(id);
        if (moviedto == null)
        {
            return NotFound("Movie not found");
        }
        return Ok(moviedto);
    }

    [Authorize(Roles = "Admin"), HttpPut("{id}")]
    public async Task<ActionResult<Movies?>> updatemovie(int id,CreateMovieDto createMovieDto)
    {
        var movie= await _moviesService.updateMovie(id, createMovieDto);
        if (movie == null)
        {
            return NotFound("Movie not found not able to update");
        }
        return Ok("movie updated\n"+movie);
    }
    
    [Authorize(Roles = "Admin"), HttpDelete("{id}")]
    public async Task<ActionResult<bool>> deleteMovie(int id)
    {
        bool isdeleted=await _moviesService.DeleteMovieAsync(id);
        if (!isdeleted)
        {
            return NoContent();
        }
        return Ok("Movie deleted");
    }
    //using query params
    [HttpGet("search/")]
    public async Task<IActionResult> SearchMoviesByTitle([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest("Movie title cannot be empty.");
        }

        var movies = await _moviesService.SearchMoviesByTitleAsync(title);

        if (movies.Count == 0)
        {
            return NotFound("No movies found.");
        }

        return Ok(movies);
    }
}
