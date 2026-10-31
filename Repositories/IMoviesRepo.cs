using BookMyShow.DTOs;
using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface IMoviesRepo
{
    Task AddMovieAsync(Movies  movie);
}