using BookMyShow.Data;
using BookMyShow.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repositories;

public class ShowsRepo : IShowsRepo
{
    private readonly ApplicationDbContext _dbContext;

    public ShowsRepo(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task AddAsync(Shows show, IReadOnlyCollection<ShowSeats> showSeats)
    {
        foreach (var showSeat in showSeats)
            showSeat.Show = show;

        await _dbContext.Shows.AddAsync(show);
        await _dbContext.ShowSeats.AddRangeAsync(showSeats);
        await _dbContext.SaveChangesAsync();
    }

    public Task<List<Shows>> GetAllAsync() =>
        _dbContext.Shows
            .Include(show => show.Movie)
            .Include(show => show.Screen)
                .ThenInclude(screen => screen.Theatre)
            .ToListAsync();

    public Task<Shows?> GetByIdAsync(int showId) =>
        _dbContext.Shows
            .Include(show => show.Movie)
            .Include(show => show.Screen)
                .ThenInclude(screen => screen.Theatre)
            .FirstOrDefaultAsync(show => show.ShowId == showId);

    public Task<List<Shows>> GetByMovieAsync(int movieId) =>
        _dbContext.Shows
            .Include(show => show.Movie)
            .Include(show => show.Screen)
                .ThenInclude(screen => screen.Theatre)
            .Where(show => show.MovieId == movieId)
            .ToListAsync();

    public Task<List<Shows>> GetByTheatreAsync(int theatreId) =>
        _dbContext.Shows
            .Include(show => show.Movie)
            .Include(show => show.Screen)
                .ThenInclude(screen => screen.Theatre)
            .Where(show => show.Screen.TheatreId == theatreId)
            .ToListAsync();

    public Task<List<Shows>> SearchByDateAsync(DateOnly date)
    {
        var startOfDay = date.ToDateTime(TimeOnly.MinValue);
        var startOfNextDay = date.AddDays(1).ToDateTime(TimeOnly.MinValue);

        return _dbContext.Shows
            .Include(show => show.Movie)
            .Include(show => show.Screen)
                .ThenInclude(screen => screen.Theatre)
            .Where(show => show.StartTime >= startOfDay && show.StartTime < startOfNextDay)
            .ToListAsync();
    }

    public Task<bool> HasOverlapAsync(int screensId, DateTime startTime, DateTime endTime, int? excludeShowId = null) =>
        _dbContext.Shows.AnyAsync(show =>
            show.ScreensId == screensId &&
            (!excludeShowId.HasValue || show.ShowId != excludeShowId.Value) &&
            startTime < show.EndTime && endTime > show.StartTime);

    public Task<List<int>> GetSeatIdsByScreenAsync(int screensId) =>
        _dbContext.Seats
            .Where(seat => seat.ScreensId == screensId)
            .Select(seat => seat.SeatId)
            .ToListAsync();

    public async Task UpdateAsync(Shows show)
    {
        _dbContext.Shows.Update(show);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Shows show)
    {
        _dbContext.Shows.Remove(show);
        await _dbContext.SaveChangesAsync();
    }
}
