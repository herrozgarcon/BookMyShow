using BookMyShow.Data;
using BookMyShow.DTOs;
using BookMyShow.Models;
using BookMyShow.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Services;

public class ScreenService : IScreenService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IScreenRepo _screenRepo;

    public ScreenService(
        IScreenRepo screenRepo,
        ApplicationDbContext dbContext)
    {
        _screenRepo = screenRepo;
        _dbContext = dbContext;
    }

    public async Task AddScreenAsync(CreateScreenDto screens)
    {
        var theatreExists = await _dbContext.Theatres
            .AnyAsync(t => t.TheatresId == screens.TheatreId);

        if (!theatreExists)
        {
            throw new KeyNotFoundException("Theatre does not exist.");
        }

        var screen = new Screens
        {
            TheatreId = screens.TheatreId,
            ScreenName = screens.ScreenName
        };

        await _screenRepo.AddScreenAsync(screen);
    }

    public async Task<List<Screens>> GetAllScreensAsync()
    {
        return await _screenRepo.GetAllScreensAsync();
    }

    public async Task<Screens?> GetScreenByIdAsync(int id)
    {
        return await _screenRepo.GetScreenByIdAsync(id);
    }

    public async Task UpdateScreenAsync(int id, CreateScreenDto screens)
    {
        var existingScreen = await _screenRepo.GetScreenByIdAsync(id);

        if (existingScreen == null)
        {
            throw new KeyNotFoundException("Screen not found.");
        }

        existingScreen.ScreenName = screens.ScreenName;
        existingScreen.TheatreId = screens.TheatreId;

        await _screenRepo.UpdateScreenAsync();
    }

    public async Task DeleteScreenAsync(int id)
    {
        var screen = await _screenRepo.GetScreenByIdAsync(id);

        if (screen == null)
        {
            throw new KeyNotFoundException("Screen not found.");
        }

        await _screenRepo.DeleteScreenAsync(screen);
    }
}