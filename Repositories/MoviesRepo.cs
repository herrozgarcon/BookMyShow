using BookMyShow.Data;
using BookMyShow.DTOs;
using BookMyShow.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repositories;

public class MoviesRepo : IMoviesRepo
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

    public async Task<List<Movies>> GetAllMoviesAsync()
    {
        return await _dbContext.Movies.ToListAsync();
    }

    public async Task<Movies?> GetMovieByIdAsync(int movieId)
    {
        var movie= await _dbContext.Movies.FirstOrDefaultAsync(m => m.MoviesId == movieId);
        return movie;
    }

    public async Task<Movies?> updateMovieAsync(Movies movie)
    {
        _dbContext.Movies.Update(movie);
        await _dbContext.SaveChangesAsync();
        return movie;
    }

    public async Task<bool> DeleteMovieAsync(Movies movie)
    {
        if (movie == null)
        {
            return false;
        }
        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Movies>> SearchMoviesByTitleAsync(string title)
    {
        return await _dbContext.Movies
            .Where(m => m.Title.Contains(title))
            .ToListAsync();
    }
    

}