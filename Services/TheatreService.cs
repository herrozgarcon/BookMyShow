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
       var  resultlist = theatrelist.Select(t => new TheatreDto
            {
                Id = t.TheatresId,
                Name = t.Name,
                location = t.Location
            }

        ).ToList();
        return resultlist;
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

    public async Task<TheatreDto?> GetTheatrebyID(int id)
    {
        var theatre = await _theatreRepo.getTheatrebyID(id);
        
        if (theatre == null)
        {
            return null;
        }

        return new TheatreDto
        {
            Id = theatre.TheatresId,
            Name = theatre.Name,
            location = theatre.Location
        };
    }
    
    public async Task<Theatres> updateTheatreAsync(int id,CreateTheatreDto createTheatreDto)
    {

        var theatre = await _theatreRepo.getTheatrebyID(id);
        if (theatre == null)
        {
            return null;
        }

        theatre.Name = createTheatreDto.Name;
        theatre.Location = createTheatreDto.Location;
        
        await _theatreRepo.updateTheatreAsync(theatre);
        return theatre;
    }

    public async Task<bool> deleteTheatre(int id)
    {
        return await _theatreRepo.deleteTheatre(id);
         
    }
    
    
}