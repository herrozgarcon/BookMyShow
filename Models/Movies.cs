namespace BookMyShow.Models;

public class Movies
{
    public int MoviesId { get; set; }
  
    public string Title { get; set; }
    
    public string Description{ get; set; }
    
    public int DurationMinutes{ get; set; }
    
    public DateOnly  ReleaseDate { get; set; }
    
    public string Language{ get; set; }
    
    public string Genre { get; set; }
    
}