using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface ITheatreRepo
{
   Task AddTheatreasync(Theatres theatres);
   
   Task<List<Theatres>> GetAllTheatres();
   
   Task<Theatres> getTheatrebyID(int id);
   
   Task updateTheatreAsync(Theatres theatres);
   
   Task<bool> deleteTheatre(int id);
}