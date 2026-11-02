using BookMyShow.DTOs;
using BookMyShow.Models;
using BookMyShow.Repositories;

namespace BookMyShow.Services;

public class SeatsService : ISeatsService
{
    private readonly ISeatsRepo _seatsRepo;
    public SeatsService(ISeatsRepo seatsRepo) => _seatsRepo = seatsRepo;

    public async Task<SeatResponseDto> AddAsync(CreateSeatDto dto)
    {
        Validate(dto.RowName, dto.SeatNumber, dto.SeatType);
        await EnsureScreen(dto.ScreensId);
        if (await _seatsRepo.PositionExistsAsync(dto.ScreensId, dto.RowName, dto.SeatNumber))
            throw new InvalidOperationException("A seat already exists at that position.");
        var seat = new Seats { ScreensId = dto.ScreensId, RowName = dto.RowName.Trim(), SeatNumber = dto.SeatNumber, SeatType = dto.SeatType.Trim() };
        await _seatsRepo.AddAsync(seat);
        return ToDto(seat);
    }

    public async Task<List<SeatResponseDto>> AddBulkAsync(BulkCreateSeatsDto dto)
    {
        if (dto.Seats is null || dto.Seats.Count == 0) throw new ArgumentException("At least one seat is required.");
        await EnsureScreen(dto.ScreensId);
        foreach (var item in dto.Seats) Validate(item.RowName, item.SeatNumber, item.SeatType);
        var incoming = dto.Seats.Select(x => (Row: x.RowName.Trim(), x.SeatNumber)).ToList();
        if (incoming.Distinct().Count() != incoming.Count) throw new InvalidOperationException("The request contains duplicate seat positions.");
        foreach (var (row, number) in incoming)
            if (await _seatsRepo.PositionExistsAsync(dto.ScreensId, row, number))
                throw new InvalidOperationException($"Seat position {row}{number} already exists.");
        var seats = dto.Seats.Select(x => new Seats { ScreensId = dto.ScreensId, RowName = x.RowName.Trim(), SeatNumber = x.SeatNumber, SeatType = x.SeatType.Trim() }).ToList();
        await _seatsRepo.AddRangeAsync(seats);
        return seats.Select(ToDto).ToList();
    }

    public async Task<List<SeatResponseDto>> GetByScreenAsync(int screensId)
    {
        await EnsureScreen(screensId);
        return (await _seatsRepo.GetByScreenAsync(screensId)).Select(ToDto).ToList();
    }

    public async Task<SeatResponseDto> GetByIdAsync(int seatId) => ToDto(await FindSeat(seatId));

    public async Task<SeatResponseDto> UpdateAsync(int seatId, UpdateSeatDto dto)
    {
        var seat = await FindSeat(seatId);
        Validate(dto.RowName, dto.SeatNumber, dto.SeatType);
        var row = dto.RowName.Trim();
        if (await _seatsRepo.PositionExistsAsync(seat.ScreensId, row, dto.SeatNumber, seatId))
            throw new InvalidOperationException("A seat already exists at that position.");
        seat.RowName = row;
        seat.SeatNumber = dto.SeatNumber;
        seat.SeatType = dto.SeatType.Trim();
        await _seatsRepo.UpdateAsync(seat);
        return ToDto(seat);
    }

    public async Task DeleteAsync(int seatId) => await _seatsRepo.DeleteAsync(await FindSeat(seatId));

    private async Task EnsureScreen(int screensId)
    {
        if (screensId <= 0 || !await _seatsRepo.ScreenExistsAsync(screensId)) throw new KeyNotFoundException("Screen not found.");
    }

    private async Task<Seats> FindSeat(int id) => await _seatsRepo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Seat not found.");

    private static void Validate(string row, int number, string type)
    {
        if (string.IsNullOrWhiteSpace(row) || row.Trim().Length > 5) throw new ArgumentException("RowName is required and must be at most 5 characters.");
        if (number <= 0) throw new ArgumentException("SeatNumber must be greater than 0.");
        if (string.IsNullOrWhiteSpace(type) || type.Trim().Length > 30) throw new ArgumentException("SeatType is required and must be at most 30 characters.");
    }

    private static SeatResponseDto ToDto(Seats s) => new() { SeatId = s.SeatId, ScreensId = s.ScreensId, RowName = s.RowName, SeatNumber = s.SeatNumber, SeatType = s.SeatType };
}
