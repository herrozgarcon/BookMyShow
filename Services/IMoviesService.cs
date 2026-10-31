using BookMyShow.DTOs;
using BookMyShow.Migrations;

namespace BookMyShow.Services;

public interface IMoviesService
{
    public Task AddMovieAsync(CreateMovieDto createMovieDto);
    
}