using BookMyShow.DTOs;
using BookMyShow.Models;

namespace BookMyShow.Services;

public interface ITheatreService
{
    
    Task  AddTheatre(CreateTheatreDto createTheatreDto);
    
    Task<List<TheatreDto>> GetAllTheatres();
    
}