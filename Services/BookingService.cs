using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using BookMyShow.DTOs;
using BookMyShow.Models;
using BookMyShow.Repositories;

namespace BookMyShow.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepo _repo;
    public BookingService(IBookingRepo repo) => _repo = repo;

    public async Task<BookingResponseDto?> CreateAsync(BookingRequestDto request, CancellationToken cancellationToken = default)
    {
        var name = request.CustomerName?.Trim() ?? "";
        var email = request.CustomerEmail?.Trim() ?? "";
        var phone = request.CustomerPhone?.Trim() ?? "";
        if (name.Length == 0 || name.Length > 100 || !new EmailAddressAttribute().IsValid(email) || email.Length > 150 ||
            !Regex.IsMatch(phone, @"^\+?[0-9]{7,15}$")) throw new ArgumentException("Customer name, email, or phone is invalid.");
        if (request.SeatIds == null || request.SeatIds.Count == 0) throw new ArgumentException("At least one seat must be selected.");
        if (request.SeatIds.Any(x => x <= 0) || request.SeatIds.Distinct().Count() != request.SeatIds.Count)
            throw new ArgumentException("Seat IDs must be positive and unique.");
        var booking = new Booking { ShowId = request.ShowId, CustomerName = name, CustomerEmail = email,
            CustomerPhone = phone, BookingDate = DateTime.UtcNow, Status = "Confirmed" };
        Booking? created = null;
        for (var attempt = 0; attempt < 3; attempt++)
        {
            booking.BookingReference = $"BMS-{DateTime.UtcNow:yyyyMMdd}-{Convert.ToHexString(RandomNumberGenerator.GetBytes(4))}";
            created = await _repo.CreateAsync(booking, request.SeatIds, cancellationToken);
            if (created != null) break;
        }
        return created == null ? null : Map(created, "Booking created successfully");
    }

    public async Task<List<BookingResponseDto>> GetAllAsync(CancellationToken ct = default) => (await _repo.GetAllAsync(ct)).Select(x => Map(x)).ToList();
    public async Task<BookingResponseDto?> GetByIdAsync(int id, CancellationToken ct = default) { var x = await _repo.GetByIdAsync(id, ct); return x == null ? null : Map(x); }
    public async Task<BookingResponseDto?> GetByReferenceAsync(string reference, CancellationToken ct = default) { var x = await _repo.GetByReferenceAsync(reference, ct); return x == null ? null : Map(x); }
    public async Task<List<BookingResponseDto>> GetByShowAsync(int showId, CancellationToken ct = default) => (await _repo.GetByShowAsync(showId, ct)).Select(x => Map(x)).ToList();

    public async Task<BookingCancellationResponseDto?> CancelAsync(int id, CancellationToken ct = default)
    {
        var booking = await _repo.CancelAsync(id, ct);
        return booking == null ? null : new BookingCancellationResponseDto
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            Status = booking.Status,
            ReleasedSeats = booking.Seats.Select(seat => new ReleasedSeatDto { SeatId = seat.SeatId }).ToList()
        };
    }

    private static BookingResponseDto Map(Booking x, string? message = null) => new()
    {
        Message = message, BookingId = x.BookingId, BookingReference = x.BookingReference, ShowId = x.ShowId,
        CustomerName = x.CustomerName, CustomerEmail = x.CustomerEmail, CustomerPhone = x.CustomerPhone,
        BookingDate = x.BookingDate, TotalAmount = x.TotalAmount, Status = x.Status,
        Seats = x.Seats.Select(s => new BookingSeatResponseDto { SeatId = s.SeatId, Price = s.Price }).ToList()
    };
}
