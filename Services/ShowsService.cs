using BookMyShow.DTOs;
using BookMyShow.Models;
using BookMyShow.Repositories;

namespace BookMyShow.Services;

public class ShowsService : IShowsService
{
    private readonly IShowsRepo _showsRepo;
    private readonly IMoviesRepo _moviesRepo;
    private readonly IScreenRepo _screenRepo;

    public ShowsService(IShowsRepo showsRepo, IMoviesRepo moviesRepo, IScreenRepo screenRepo)
    {
        _showsRepo = showsRepo;
        _moviesRepo = moviesRepo;
        _screenRepo = screenRepo;
    }

    public async Task<Shows> CreateAsync(CreateShowDto dto)
    {
        ValidateTimesAndPrice(dto.StartTime, dto.EndTime, dto.TicketPrice);
        await EnsureMovieAndScreen(dto.MovieId, dto.ScreensId);

        if (await _showsRepo.HasOverlapAsync(dto.ScreensId, dto.StartTime, dto.EndTime))
            throw new InvalidOperationException("The requested show time overlaps another show on this screen.");

        var show = new Shows
        {
            MovieId = dto.MovieId,
            ScreensId = dto.ScreensId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            TicketPrice = dto.TicketPrice
        };

        var showSeats = (await _showsRepo.GetSeatIdsByScreenAsync(dto.ScreensId))
            .Select(seatId => new ShowSeats { SeatId = seatId, Status = "Available" })
            .ToList();

        await _showsRepo.AddAsync(show, showSeats);
        return show;
    }

    public Task<List<Shows>> GetAllAsync() => _showsRepo.GetAllAsync();

    public Task<Shows?> GetByIdAsync(int showId) => _showsRepo.GetByIdAsync(showId);

    public Task<List<Shows>> GetByMovieAsync(int movieId) => _showsRepo.GetByMovieAsync(movieId);

    public Task<List<Shows>> GetByTheatreAsync(int theatreId) => _showsRepo.GetByTheatreAsync(theatreId);

    public Task<List<Shows>> SearchByDateAsync(DateOnly date) => _showsRepo.SearchByDateAsync(date);

    public async Task<Shows?> UpdateAsync(int showId, CreateShowDto dto)
    {
        var show = await _showsRepo.GetByIdAsync(showId);
        if (show == null)
            return null;

        ValidateTimesAndPrice(dto.StartTime, dto.EndTime, dto.TicketPrice);
        await EnsureMovieAndScreen(dto.MovieId, dto.ScreensId);

        if (await _showsRepo.HasOverlapAsync(dto.ScreensId, dto.StartTime, dto.EndTime, showId))
            throw new InvalidOperationException("The requested show time overlaps another show on this screen.");

        show.MovieId = dto.MovieId;
        show.ScreensId = dto.ScreensId;
        show.StartTime = dto.StartTime;
        show.EndTime = dto.EndTime;
        show.TicketPrice = dto.TicketPrice;

        await _showsRepo.UpdateAsync(show);
        return show;
    }

    public async Task<bool> DeleteAsync(int showId)
    {
        var show = await _showsRepo.GetByIdAsync(showId);
        if (show == null)
            return false;

        await _showsRepo.DeleteAsync(show);
        return true;
    }

    private async Task EnsureMovieAndScreen(int movieId, int screensId)
    {
        if (movieId <= 0 || await _moviesRepo.GetMovieByIdAsync(movieId) == null)
            throw new KeyNotFoundException("Movie not found.");

        if (screensId <= 0 || await _screenRepo.GetScreenByIdAsync(screensId) == null)
            throw new KeyNotFoundException("Screen not found.");
    }

    private static void ValidateTimesAndPrice(DateTime startTime, DateTime endTime, decimal ticketPrice)
    {
        if (endTime <= startTime)
            throw new ArgumentException("EndTime must be later than StartTime.");

        if (ticketPrice < 0)
            throw new ArgumentException("TicketPrice cannot be negative.");
    }
}
