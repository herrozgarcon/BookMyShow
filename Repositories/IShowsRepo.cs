using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface IShowsRepo
{
    Task AddAsync(Shows show, IReadOnlyCollection<ShowSeats> showSeats);

    Task<List<Shows>> GetAllAsync();

    Task<Shows?> GetByIdAsync(int showId);

    Task<List<Shows>> GetByMovieAsync(int movieId);

    Task<List<Shows>> GetByTheatreAsync(int theatreId);

    Task<List<Shows>> SearchByDateAsync(DateOnly date);

    Task<bool> HasOverlapAsync(int screensId, DateTime startTime, DateTime endTime, int? excludeShowId = null);

    Task<List<int>> GetSeatIdsByScreenAsync(int screensId);

    Task UpdateAsync(Shows show);

    Task DeleteAsync(Shows show);
}
