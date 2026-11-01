using BookMyShow.DTOs;
using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface IMoviesRepo
{
    Task AddMovieAsync(Movies  movie);
    Task<List<Movies>> GetAllMoviesAsync();
    
    Task<Movies?> GetMovieByIdAsync(int movieId);
    
    Task<Movies?> updateMovieAsync(Movies movie);
    
    Task<bool> DeleteMovieAsync(Movies movie);
    
    
    
    
}