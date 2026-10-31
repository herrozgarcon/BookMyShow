using BookMyShow.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Data;

public class ApplicationDbContext: DbContext
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    //creating the table 
    public DbSet<Theatres> Theatres { get; set; }
    
    
    //setting up contraints for the table to be created 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Theatres>(entity =>
        {
            entity.HasKey(t => t.TheatresId);

            entity.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(t => t.Location)
                .IsRequired()
                .HasMaxLength(200);
        });
        
        
        
    }
    
}