namespace BookMyShow.DTOs;

public class MovieDto
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string Genre { get; set; }
    
    public DateOnly releaseDate { get; set; }
    
    public int duration { get; set; }
    
    public string language { get; set; }
}