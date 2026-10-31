using BookMyShow.DTOs;
using BookMyShow.Models;
using BookMyShow.Repositories;

namespace BookMyShow.Services;

public class TheatreService:ITheatreService
{

    private readonly TheatreRepo _theatreRepo;
    

    public TheatreService(TheatreRepo theatreRepo)
    {
        _theatreRepo = theatreRepo;
    }


    public async Task<List<TheatreDto>> GetAllTheatres()
    {
        var theatrelist = await _theatreRepo.GetAllTheatres();
        return theatrelist.Select(t => new TheatreDto
            {
                Id = t.TheatresId,
                Name = t.Name,
                location = t.Location
            }

        ).ToList();
    }

    public async Task AddTheatre(CreateTheatreDto createTheatreDto)
    {
        var theatre = new Theatres
        {
            Name = createTheatreDto.Name,
            Location = createTheatreDto.Location
        };

        await _theatreRepo.AddTheatreasync(theatre);
    }
    
    
    
    
}