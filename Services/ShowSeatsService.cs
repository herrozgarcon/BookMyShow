using BookMyShow.Models;
using BookMyShow.Repositories;

namespace BookMyShow.Services;

public class ShowSeatsService : IShowSeatsService
{
    private readonly IShowSeatsRepo _showSeatsRepo;
    private readonly IShowsRepo _showsRepo;

    public ShowSeatsService(IShowSeatsRepo showSeatsRepo, IShowsRepo showsRepo)
    {
        _showSeatsRepo = showSeatsRepo;
        _showsRepo = showsRepo;
    }

    public async Task<List<ShowSeats>> GetByShowAsync(int showId)
    {
        await EnsureShowExists(showId);
        return await _showSeatsRepo.GetByShowAsync(showId);
    }

    public async Task<List<ShowSeats>> GetAvailableByShowAsync(int showId)
    {
        await EnsureShowExists(showId);
        return await _showSeatsRepo.GetAvailableByShowAsync(showId);
    }

    public async Task<List<ShowSeats>> GetBookedByShowAsync(int showId)
    {
        await EnsureShowExists(showId);
        return await _showSeatsRepo.GetBookedByShowAsync(showId);
    }

    private async Task EnsureShowExists(int showId)
    {
        if (showId <= 0 || await _showsRepo.GetByIdAsync(showId) == null)
            throw new KeyNotFoundException("Show not found.");
    }
}
