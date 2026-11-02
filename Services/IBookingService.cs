using BookMyShow.DTOs;

namespace BookMyShow.Services;

public interface IBookingService
{
    Task<BookingResponseDto?> CreateAsync(BookingRequestDto request, CancellationToken cancellationToken = default);
    Task<List<BookingResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BookingResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BookingResponseDto?> GetByReferenceAsync(string reference, CancellationToken cancellationToken = default);
    Task<List<BookingResponseDto>> GetByShowAsync(int showId, CancellationToken cancellationToken = default);
    Task<BookingCancellationResponseDto?> CancelAsync(int id, CancellationToken cancellationToken = default);
}
