using BookMyShow.Data;
using BookMyShow.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repositories;

public class SeatsRepo : ISeatsRepo
{
    private readonly ApplicationDbContext _dbContext;
    public SeatsRepo(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task AddAsync(Seats seat)
    {
        await _dbContext.Seats.AddAsync(seat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IReadOnlyCollection<Seats> seats)
    {
        await _dbContext.Seats.AddRangeAsync(seats);
        await _dbContext.SaveChangesAsync();
    }

    public Task<List<Seats>> GetByScreenAsync(int screensId) =>
        _dbContext.Seats.Where(s => s.ScreensId == screensId)
            .OrderBy(s => s.RowName).ThenBy(s => s.SeatNumber).ToListAsync();

    public Task<Seats?> GetByIdAsync(int seatId) =>
        _dbContext.Seats.FirstOrDefaultAsync(s => s.SeatId == seatId);

    public async Task UpdateAsync(Seats seat)
    {
        _dbContext.Seats.Update(seat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Seats seat)
    {
        _dbContext.Seats.Remove(seat);
        await _dbContext.SaveChangesAsync();
    }

    public Task<bool> PositionExistsAsync(int screensId, string rowName, int seatNumber, int? excludeSeatId = null) =>
        _dbContext.Seats.AnyAsync(s => s.ScreensId == screensId && s.RowName == rowName &&
                                       s.SeatNumber == seatNumber && (!excludeSeatId.HasValue || s.SeatId != excludeSeatId.Value));

    public Task<bool> ScreenExistsAsync(int screensId) =>
        _dbContext.Screens.AnyAsync(s => s.ScreensId == screensId);
}
