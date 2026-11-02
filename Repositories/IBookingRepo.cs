using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface IBookingRepo
{
    Task<Booking?> CreateAsync(Booking booking, IReadOnlyCollection<int> seatIds, CancellationToken cancellationToken = default);
    Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Booking?> GetByReferenceAsync(string reference, CancellationToken cancellationToken = default);
    Task<List<Booking>> GetByShowAsync(int showId, CancellationToken cancellationToken = default);
    Task<Booking?> CancelAsync(int id, CancellationToken cancellationToken = default);
}
