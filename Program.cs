using BookMyShow.Data;
using BookMyShow.Services;
using Microsoft.EntityFrameworkCore;
using BookMyShow.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi



builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
// Theatre (automated object creation)
builder.Services.AddScoped<ITheatreRepo,TheatreRepo>();
builder.Services.AddScoped<ITheatreService, TheatreService>();
// Movie DI (automated object creation)
builder.Services.AddScoped<IMoviesRepo, MoviesRepo>();
builder.Services.AddScoped<IMoviesService, MoviesService>();
//Screen Di(automated object creation)
builder.Services.AddScoped<IScreenService, ScreenService>();
builder.Services.AddScoped<IScreenRepo, ScreenRepo>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
