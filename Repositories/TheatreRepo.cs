using BookMyShow.Data;
using BookMyShow.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repositories;

public class TheatreRepo: ITheatreRepo
{
    
    private readonly ApplicationDbContext _dbContext;
    public TheatreRepo(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddTheatreasync(Theatres theatres)
    {
        await _dbContext.Theatres.AddAsync(theatres);
        await _dbContext.SaveChangesAsync();
    }


    public async Task<List<Theatres>> GetAllTheatres()
    {
        var theatreslist = await _dbContext.Theatres.ToListAsync();
        return theatreslist;
    }
    
    
}