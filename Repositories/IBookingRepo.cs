using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface IBookingRepo
{
    Task<Booking?> CreateAsync(Booking booking, IReadOnlyCollection<int> seatIds, CancellationToken cancellationToken = default);
    Task<List<Booking>> GetAllAsync(int userId, CancellationToken cancellationToken = default);
    Task<Booking?> GetByIdAsync(int id, int userId, CancellationToken cancellationToken = default);
    Task<Booking?> GetByReferenceAsync(string reference, int userId, CancellationToken cancellationToken = default);
    Task<List<Booking>> GetByShowAsync(int showId, int userId, CancellationToken cancellationToken = default);
    Task<Booking?> CancelAsync(int id, int userId, CancellationToken cancellationToken = default);
}
