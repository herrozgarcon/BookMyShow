using BookMyShow.Data;
using BookMyShow.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repositories;

public class ScreenRepo : IScreenRepo
{
    private readonly ApplicationDbContext _dbContext;

    public ScreenRepo(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddScreenAsync(Screens screen)
    {
        await _dbContext.Screens.AddAsync(screen);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Screens>> GetAllScreensAsync()
    {
        return await _dbContext.Screens.ToListAsync();
    }

    public async Task<Screens?> GetScreenByIdAsync(int id)
    {
        return await _dbContext.Screens
            .FirstOrDefaultAsync(s => s.ScreensId == id);
    }

    public async Task UpdateScreenAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteScreenAsync(Screens screen)
    {
        _dbContext.Screens.Remove(screen);
        await _dbContext.SaveChangesAsync();
    }
}