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
    public DbSet<Movies> Movies { get; set; }
    
    public DbSet<Screens> Screens{ get; set; }
    
    
    
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

        modelBuilder.Entity<Movies>(entity =>
        {
            entity.HasKey(m => m.MoviesId);

            entity.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(m => m.ReleaseDate)
                .IsRequired()
                .HasColumnType("date");
            entity.Property(m => m.Genre)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(m=>m.Description)
                .IsRequired()
                .HasMaxLength(200);
                
        });
        
        modelBuilder.Entity<Screens>(entity =>
        {
            entity.HasKey(s => s.ScreensId);

            entity.Property(s => s.ScreenName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(s => s.Theatre)
                .WithMany()
                .HasForeignKey(s => s.TheatreId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(s => new { s.TheatreId, s.ScreenName })
                .IsUnique();
        });



    }
    
}