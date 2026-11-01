using BookMyShow.DTOs;
using BookMyShow.Models;

namespace BookMyShow.Services;

public interface IMoviesService
{
    public Task AddMovieAsync(CreateMovieDto createMovieDto);
    
    public Task<List<MovieDto>> GetAllMoviesAsync();
    
    public Task<MovieDto?> GetMovieByIdAsync(int id);
    
    public Task<Movies?> updateMovie(int id,CreateMovieDto createMovieDto);
    
    public Task<bool> DeleteMovieAsync(int id);
    
    Task<List<Movies>> SearchMoviesByTitleAsync(string title);
    
}