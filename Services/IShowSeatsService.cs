using BookMyShow.Models;

namespace BookMyShow.Services;

public interface IShowSeatsService
{
    Task<List<ShowSeats>> GetByShowAsync(int showId);

    Task<List<ShowSeats>> GetAvailableByShowAsync(int showId);

    Task<List<ShowSeats>> GetBookedByShowAsync(int showId);
}
