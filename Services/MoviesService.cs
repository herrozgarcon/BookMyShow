using BookMyShow.DTOs;
using BookMyShow.Models;
using BookMyShow.Repositories;

namespace BookMyShow.Services;

public class MoviesService:IMoviesService
{
    private readonly IMoviesRepo _moviesRepo;

    public MoviesService(IMoviesRepo moviesRepo)
    {
        _moviesRepo = moviesRepo;
    }
    public async Task AddMovieAsync(CreateMovieDto createMovieDto)
    {
        var Movies = new Models.Movies
        {
            Title = createMovieDto.Title,
            Description = createMovieDto.Description,
            DurationMinutes = createMovieDto.Durationminutes,
            Genre = createMovieDto.Genre,
            Language = createMovieDto.language,
            ReleaseDate = createMovieDto.ReleaseDate
        };
         await _moviesRepo.AddMovieAsync(Movies);
       ;
    }

    public async Task<List<MovieDto>> GetAllMoviesAsync()
    {

        var movie = await _moviesRepo.GetAllMoviesAsync();
        var movielist=movie.Select(m => new MovieDto
        {
            Id = m.MoviesId,
            Title = m.Title,
            releaseDate = m.ReleaseDate,
            Genre = m.Genre,
            language = m.Language,
            duration = m.DurationMinutes,
            Description = m.Description,
        }).ToList();
        return movielist;
    }

    public async Task<MovieDto?> GetMovieByIdAsync(int movieId)
    {
        var movie = await _moviesRepo.GetMovieByIdAsync(movieId);
        if (movie == null)
        {
            return null;
        }
        return new MovieDto
        {
            Id = movie.MoviesId,
            Title = movie.Title,
            Description = movie.Description,
            Genre = movie.Genre,
            releaseDate = movie.ReleaseDate,
            language = movie.Language,
            duration = movie.DurationMinutes,

        };
    }

    public async Task<Movies?> updateMovie(int movieId,CreateMovieDto movieDto)
    {
        var movie =await _moviesRepo.GetMovieByIdAsync(movieId);

        if (movie == null)
        {
            return null;
        }

        movie.Title = movieDto.Title;
        movie.Description = movieDto.Description;
        movie.DurationMinutes = movieDto.Durationminutes;
        movie.Genre = movieDto.Genre;
        movie.Language = movieDto.language;
        movie.ReleaseDate = movieDto.ReleaseDate;
        
        
        var resmovie=await _moviesRepo.updateMovieAsync(movie);
        return resmovie;
    }

    public async Task<bool> DeleteMovieAsync(int movieId)
    {
        var movie = await _moviesRepo.GetMovieByIdAsync(movieId);
        if (movie == null)
        {
            return false;
        }

        await _moviesRepo.DeleteMovieAsync(movie);
        return true;
    }

}