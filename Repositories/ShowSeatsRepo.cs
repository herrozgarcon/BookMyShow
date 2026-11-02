using BookMyShow.Data;
using BookMyShow.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repositories;

public class ShowSeatsRepo : IShowSeatsRepo
{
    private readonly ApplicationDbContext _dbContext;

    public ShowSeatsRepo(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public Task<List<ShowSeats>> GetByShowAsync(int showId) =>
        GetShowSeats(showId).ToListAsync();

    public Task<List<ShowSeats>> GetAvailableByShowAsync(int showId) =>
        GetShowSeats(showId)
            .Where(showSeat => showSeat.Status == "Available")
            .ToListAsync();

    public Task<List<ShowSeats>> GetBookedByShowAsync(int showId) =>
        GetShowSeats(showId)
            .Where(showSeat => showSeat.Status == "Booked")
            .ToListAsync();

    private IQueryable<ShowSeats> GetShowSeats(int showId) =>
        _dbContext.ShowSeats
            .Where(showSeat => showSeat.ShowId == showId)
            .Include(showSeat => showSeat.Seat)
            .OrderBy(showSeat => showSeat.Seat.RowName)
            .ThenBy(showSeat => showSeat.Seat.SeatNumber);
}
