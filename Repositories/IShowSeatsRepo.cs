using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface IShowSeatsRepo
{
    Task<List<ShowSeats>> GetByShowAsync(int showId);

    Task<List<ShowSeats>> GetAvailableByShowAsync(int showId);

    Task<List<ShowSeats>> GetBookedByShowAsync(int showId);
}
