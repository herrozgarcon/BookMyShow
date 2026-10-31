using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface ITheatreRepo
{
   Task AddTheatreasync(Theatres theatres);
   
   Task<List<Theatres>> GetAllTheatres();
   
}