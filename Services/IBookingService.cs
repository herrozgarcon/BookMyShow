using BookMyShow.DTOs;

namespace BookMyShow.Services;

public interface IBookingService
{
    Task<BookingResponseDto?> CreateAsync(int userId, BookingRequestDto request, CancellationToken cancellationToken = default);
    Task<List<BookingResponseDto>> GetAllAsync(int userId, CancellationToken cancellationToken = default);
    Task<BookingResponseDto?> GetByIdAsync(int id, int userId, CancellationToken cancellationToken = default);
    Task<BookingResponseDto?> GetByReferenceAsync(string reference, int userId, CancellationToken cancellationToken = default);
    Task<List<BookingResponseDto>> GetByShowAsync(int showId, int userId, CancellationToken cancellationToken = default);
    Task<BookingCancellationResponseDto?> CancelAsync(int id, int userId, CancellationToken cancellationToken = default);
}
