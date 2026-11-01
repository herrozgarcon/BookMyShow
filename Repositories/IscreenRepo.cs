using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface IScreenRepo
{
    Task AddScreenAsync(Screens screen);

    Task<List<Screens>> GetAllScreensAsync();

    Task<Screens?> GetScreenByIdAsync(int id);

    Task UpdateScreenAsync();

    Task DeleteScreenAsync(Screens screen);
}