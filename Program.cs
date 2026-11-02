using BookMyShow.Data;
using BookMyShow.Services;
using Microsoft.EntityFrameworkCore;
using BookMyShow.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Security.Claims;
using BookMyShow.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi



builder.Services.AddControllers();

var jwt = builder.Configuration.GetSection("jwtConfig");
var signingKey = jwt["Key"];
if (string.IsNullOrWhiteSpace(signingKey) || Encoding.UTF8.GetByteCount(signingKey) < 32 || signingKey.StartsWith("<", StringComparison.Ordinal))
    throw new InvalidOperationException("Configure jwtConfig:Key with a secret of at least 32 bytes using user-secrets or an environment variable.");
var issuer = jwt["Issuer"] ?? throw new InvalidOperationException("Configure jwtConfig:Issuer.");
var audience = jwt["Audience"] ?? throw new InvalidOperationException("Configure jwtConfig:Audience.");
var tokenMinutes = jwt.GetValue<int>("TokenValidityMins");
if (tokenMinutes is < 1 or > 60) throw new InvalidOperationException("jwtConfig:TokenValidityMins must be between 1 and 60.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = issuer,
        ValidAudience = audience,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.NameIdentifier,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

// Theatre (automated object creation)
builder.Services.AddScoped<ITheatreRepo,TheatreRepo>();
builder.Services.AddScoped<ITheatreService, TheatreService>();
// Movie DI (automated object creation)
builder.Services.AddScoped<IMoviesRepo, MoviesRepo>();
builder.Services.AddScoped<IMoviesService, MoviesService>();
//Screen Di(automated object creation)
builder.Services.AddScoped<IScreenService, ScreenService>();
builder.Services.AddScoped<IScreenRepo, ScreenRepo>();
builder.Services.AddScoped<ISeatsRepo, SeatsRepo>();
builder.Services.AddScoped<ISeatsService, SeatsService>();
builder.Services.AddScoped<IShowsRepo, ShowsRepo>();
builder.Services.AddScoped<IShowsService, ShowsService>();
builder.Services.AddScoped<IShowSeatsRepo, ShowSeatsRepo>();
builder.Services.AddScoped<IShowSeatsService, ShowSeatsService>();
builder.Services.AddScoped<IBookingRepo, BookingRepo>();
builder.Services.AddScoped<IBookingService, BookingService>();


builder.Services.AddAuthorization();
var app = builder.Build();

if (args.Contains("--provision-admin", StringComparer.Ordinal))
{
    var adminEmail = builder.Configuration["AdminProvisioning:Email"] ?? throw new InvalidOperationException("Set AdminProvisioning:Email through user-secrets or environment variables.");
    var adminPassword = builder.Configuration["AdminProvisioning:Password"] ?? throw new InvalidOperationException("Set AdminProvisioning:Password through user-secrets or environment variables.");
    await using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var normalizedEmail = adminEmail.Trim().ToUpperInvariant();
    if (!await db.AppUsers.AnyAsync(u => u.NormalizedEmail == normalizedEmail))
    {
        var admin = new AppUser { Name = "Administrator", Email = adminEmail.Trim(), NormalizedEmail = normalizedEmail, Role = "Admin" };
        admin.PasswordHash = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>().HashPassword(admin, adminPassword);
        db.AppUsers.Add(admin);
        await db.SaveChangesAsync();
    }
    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
