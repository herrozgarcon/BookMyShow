using System.Data;
using BookMyShow.Data;
using BookMyShow.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repositories;

public class BookingRepo : IBookingRepo
{
    private readonly ApplicationDbContext _db;
    public BookingRepo(ApplicationDbContext db) => _db = db;

    public async Task<Booking?> CreateAsync(Booking booking, IReadOnlyCollection<int> seatIds, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var show = await _db.Shows.AsNoTracking().FirstOrDefaultAsync(x => x.ShowId == booking.ShowId, cancellationToken);
        if (show == null) return null;
        var seats = await _db.Seats.AsNoTracking().Where(x => seatIds.Contains(x.SeatId)).ToListAsync(cancellationToken);
        if (seats.Count != seatIds.Count || seats.Any(x => x.ScreensId != show.ScreensId))
            throw new ArgumentException("One or more selected seats do not belong to this show's screen.");

        var requestedShowSeats = _db.ShowSeats.Where(x => x.ShowId == show.ShowId && seatIds.Contains(x.SeatId));
        var showSeatCount = await requestedShowSeats.CountAsync(cancellationToken);
        if (showSeatCount != seatIds.Count) throw new ArgumentException("A selected seat is not available for this show.");
        var affected = await requestedShowSeats.Where(x => x.Status == "Available")
            .ExecuteUpdateAsync(update => update.SetProperty(x => x.Status, "Booked"), cancellationToken);
        if (affected != seatIds.Count) throw new InvalidOperationException("One or more selected seats are already booked.");

        booking.TotalAmount = show.TicketPrice * seatIds.Count;
        foreach (var seatId in seatIds) booking.Seats.Add(new BookingSeat { SeatId = seatId, Price = show.TicketPrice });
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return booking;
    }

    private IQueryable<Booking> Query() => _db.Bookings.AsNoTracking().Include(x => x.Show).ThenInclude(x => x.Movie)
        .Include(x => x.Show).ThenInclude(x => x.Screen).ThenInclude(x => x.Theatre)
        .Include(x => x.Seats).ThenInclude(x => x.Seat);

    public Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken = default) => Query().OrderByDescending(x => x.BookingDate).ToListAsync(cancellationToken);
    public Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default) => Query().FirstOrDefaultAsync(x => x.BookingId == id, cancellationToken);
    public Task<Booking?> GetByReferenceAsync(string reference, CancellationToken cancellationToken = default) => Query().FirstOrDefaultAsync(x => x.BookingReference == reference, cancellationToken);
    public Task<List<Booking>> GetByShowAsync(int showId, CancellationToken cancellationToken = default) => Query().Where(x => x.ShowId == showId).OrderByDescending(x => x.BookingDate).ToListAsync(cancellationToken);

    public async Task<Booking?> CancelAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var booking = await _db.Bookings.AsNoTracking().Include(x => x.Seats)
            .FirstOrDefaultAsync(x => x.BookingId == id, cancellationToken);
        if (booking == null) return null;
        if (booking.Status != "Confirmed") throw new InvalidOperationException("Only confirmed bookings can be cancelled.");
        var seatIds = booking.Seats.Select(x => x.SeatId).ToArray();
        if (seatIds.Length == 0) throw new InvalidOperationException("Booking has no associated seats; cancellation was not applied.");

        var bookingUpdated = await _db.Bookings.Where(x => x.BookingId == id && x.Status == "Confirmed")
            .ExecuteUpdateAsync(update => update.SetProperty(x => x.Status, "Cancelled"), cancellationToken);
        if (bookingUpdated != 1) throw new InvalidOperationException("Booking was changed by another request. Retry the cancellation.");

        var showSeats = _db.ShowSeats.Where(x => x.ShowId == booking.ShowId && seatIds.Contains(x.SeatId));
        var matched = await showSeats.CountAsync(cancellationToken);
        if (matched != seatIds.Length) throw new InvalidOperationException("Booking seat records are inconsistent; cancellation was not applied.");
        var released = await showSeats.Where(x => x.Status == "Booked")
            .ExecuteUpdateAsync(update => update.SetProperty(x => x.Status, "Available"), cancellationToken);
        if (released != seatIds.Length) throw new InvalidOperationException("One or more booked seats are inconsistent; cancellation was not applied.");
        await transaction.CommitAsync(cancellationToken);
        booking.Status = "Cancelled";
        return booking;
    }
}
