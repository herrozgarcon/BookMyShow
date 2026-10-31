using BookMyShow.DTOs;
using BookMyShow.Migrations;
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
}