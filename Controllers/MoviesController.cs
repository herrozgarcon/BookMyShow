using BookMyShow.DTOs;
using BookMyShow.Migrations;
using BookMyShow.Services;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpPost]
    public async  Task<ActionResult> addMovie(CreateMovieDto createMovieDto)
    {
        await _moviesService.AddMovieAsync(createMovieDto);
        return Ok("Movie added");
    }
    
    
}