using BookMyShow.DTOs;
using BookMyShow.Models;

public interface IScreenService
{
    Task AddScreenAsync(CreateScreenDto screens);

    Task<List<Screens>> GetAllScreensAsync();

    Task<Screens?> GetScreenByIdAsync(int id);

    Task UpdateScreenAsync(int id, CreateScreenDto screens);

    Task<bool> DeleteScreenAsync(int id);
}