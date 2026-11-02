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
    public DbSet<Seats> Seats { get; set; }
    public DbSet<Shows> Shows { get; set; }
    public DbSet<ShowSeats> ShowSeats { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingSeat> BookingSeats { get; set; }
    
    public DbSet<AppUser>  AppUsers { get; set; }
    
    
    
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

        modelBuilder.Entity<Seats>(entity =>
        {
            entity.HasKey(s => s.SeatId);
            entity.Property(s => s.RowName).IsRequired().HasMaxLength(5);
            entity.Property(s => s.SeatNumber).IsRequired();
            entity.Property(s => s.SeatType).IsRequired().HasMaxLength(30);
            entity.HasOne(s => s.Screen)
                .WithMany()
                .HasForeignKey(s => s.ScreensId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(s => new { s.ScreensId, s.RowName, s.SeatNumber }).IsUnique();
        });

        modelBuilder.Entity<Shows>(entity =>
        {
            entity.HasKey(show => show.ShowId);
            entity.Property(show => show.StartTime).IsRequired().HasColumnType("datetime2");
            entity.Property(show => show.EndTime).IsRequired().HasColumnType("datetime2");
            entity.Property(show => show.TicketPrice).IsRequired().HasPrecision(18, 2);

            entity.HasOne(show => show.Movie)
                .WithMany()
                .HasForeignKey(show => show.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(show => show.Screen)
                .WithMany()
                .HasForeignKey(show => show.ScreensId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ShowSeats>(entity =>
        {
            entity.HasKey(showSeat => showSeat.ShowSeatId);
            entity.Property(showSeat => showSeat.Status).IsRequired().HasMaxLength(20);

            entity.HasOne(showSeat => showSeat.Show)
                .WithMany()
                .HasForeignKey(showSeat => showSeat.ShowId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(showSeat => showSeat.Seat)
                .WithMany()
                .HasForeignKey(showSeat => showSeat.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(showSeat => new { showSeat.ShowId, showSeat.SeatId }).IsUnique();
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(booking => booking.BookingId);
            entity.Property(booking => booking.BookingReference).IsRequired().HasMaxLength(30);
            entity.HasIndex(booking => booking.BookingReference).IsUnique();
            entity.Property(booking => booking.CustomerName).IsRequired().HasMaxLength(100);
            entity.Property(booking => booking.CustomerEmail).IsRequired().HasMaxLength(150);
            entity.Property(booking => booking.CustomerPhone).IsRequired().HasMaxLength(15);
            entity.Property(booking => booking.BookingDate).IsRequired().HasColumnType("datetime2");
            entity.Property(booking => booking.TotalAmount).IsRequired().HasPrecision(10, 2);
            entity.ToTable(table => table.HasCheckConstraint("CK_Bookings_TotalAmount_NonNegative", "[TotalAmount] >= 0"));
            entity.Property(booking => booking.Status).IsRequired().HasMaxLength(20);
            entity.HasOne(booking => booking.AppUser).WithMany().HasForeignKey(booking => booking.AppUserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(booking => booking.Show).WithMany(show => show.Bookings).HasForeignKey(booking => booking.ShowId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(user => user.AppUserId);
            entity.Property(user => user.Name).IsRequired().HasMaxLength(100);
            entity.Property(user => user.Email).IsRequired();
            entity.Property(user => user.NormalizedEmail).IsRequired().HasMaxLength(150);
            entity.Property(user => user.PasswordHash).IsRequired();
            entity.Property(user => user.Role).IsRequired();
            entity.HasIndex(user => user.NormalizedEmail).IsUnique();
        });

        modelBuilder.Entity<BookingSeat>(entity =>
        {
            entity.HasKey(bookingSeat => bookingSeat.BookingSeatId);
            entity.Property(bookingSeat => bookingSeat.Price).IsRequired().HasPrecision(10, 2);
            entity.ToTable(table => table.HasCheckConstraint("CK_BookingSeats_Price_NonNegative", "[Price] >= 0"));
            entity.HasOne(bookingSeat => bookingSeat.Booking).WithMany(booking => booking.Seats).HasForeignKey(bookingSeat => bookingSeat.BookingId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(bookingSeat => bookingSeat.Seat).WithMany(seat => seat.BookingSeats).HasForeignKey(bookingSeat => bookingSeat.SeatId).OnDelete(DeleteBehavior.Restrict);
        });



    }
    
}
