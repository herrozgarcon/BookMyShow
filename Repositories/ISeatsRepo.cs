using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface ISeatsRepo
{
    Task AddAsync(Seats seat);
    Task AddRangeAsync(IReadOnlyCollection<Seats> seats);
    Task<List<Seats>> GetByScreenAsync(int screensId);
    Task<Seats?> GetByIdAsync(int seatId);
    Task UpdateAsync(Seats seat);
    Task DeleteAsync(Seats seat);
    Task<bool> PositionExistsAsync(int screensId, string rowName, int seatNumber, int? excludeSeatId = null);
    Task<bool> ScreenExistsAsync(int screensId);
}
