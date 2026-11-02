using BookMyShow.DTOs;
using BookMyShow.Models;

namespace BookMyShow.Services;

public interface IShowsService
{
    Task<Shows> CreateAsync(CreateShowDto dto);

    Task<List<Shows>> GetAllAsync();

    Task<Shows?> GetByIdAsync(int showId);

    Task<List<Shows>> GetByMovieAsync(int movieId);

    Task<List<Shows>> GetByTheatreAsync(int theatreId);

    Task<List<Shows>> SearchByDateAsync(DateOnly date);

    Task<Shows?> UpdateAsync(int showId, CreateShowDto dto);

    Task<bool> DeleteAsync(int showId);
}
