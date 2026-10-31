using BookMyShow.Data;
using BookMyShow.DTOs;
using BookMyShow.Models;

namespace BookMyShow.Repositories;

public class MoviesRepo:IMoviesRepo
{
    private readonly ApplicationDbContext _dbContext;
    public MoviesRepo(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddMovieAsync(Movies movie)
    {
        await _dbContext.Movies.AddAsync(movie);
        await _dbContext.SaveChangesAsync();
        
    }
}