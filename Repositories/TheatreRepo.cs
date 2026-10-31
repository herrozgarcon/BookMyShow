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

    public async Task<Theatres> getTheatrebyID(int id)
    {
        var theatre = await _dbContext.Theatres.FirstOrDefaultAsync(th => th.TheatresId == id);
        return theatre;
    }

    public async Task updateTheatreAsync(Theatres theatres)
    {
         _dbContext.Update(theatres);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> deleteTheatre(int id)
    {
        var flag=await _dbContext.Theatres.ExecuteDeleteAsync();
        await _dbContext.SaveChangesAsync();
        if (flag == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}