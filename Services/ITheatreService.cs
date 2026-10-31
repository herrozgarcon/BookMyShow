using BookMyShow.DTOs;
using BookMyShow.Models;

namespace BookMyShow.Services;

public interface ITheatreService
{
    
    Task  AddTheatre(CreateTheatreDto createTheatreDto);
    
    Task<List<TheatreDto>> GetAllTheatres();
    
    Task<TheatreDto> GetTheatrebyID(int theatreId);

    Task<Theatres> updateTheatreAsync(int id,CreateTheatreDto theatreDto);

    Task<bool> deleteTheatre(int theatreId);
}