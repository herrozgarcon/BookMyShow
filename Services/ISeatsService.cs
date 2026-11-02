using BookMyShow.DTOs;

namespace BookMyShow.Services;

public interface ISeatsService
{
    Task<SeatResponseDto> AddAsync(CreateSeatDto dto);
    Task<List<SeatResponseDto>> AddBulkAsync(BulkCreateSeatsDto dto);
    Task<List<SeatResponseDto>> GetByScreenAsync(int screensId);
    Task<SeatResponseDto> GetByIdAsync(int seatId);
    Task<SeatResponseDto> UpdateAsync(int seatId, UpdateSeatDto dto);
    Task DeleteAsync(int seatId);
}
