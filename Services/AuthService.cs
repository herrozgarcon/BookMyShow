using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookMyShow.DTOs;
using BookMyShow.Models;
using BookMyShow.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BookMyShow.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct);
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken ct);
    Task<AppUser?> GetProfileAsync(int userId, CancellationToken ct);
    Task<AppUser?> UpdateProfileAsync(int userId, string name, CancellationToken ct);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher<AppUser> _hasher;
    private readonly IConfiguration _configuration;
    public AuthService(IUserRepository users, IPasswordHasher<AppUser> hasher, IConfiguration configuration)
    { _users = users; _hasher = hasher; _configuration = configuration; }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct)
    {
        var email = request.Email.Trim();
        var normalized = email.ToUpperInvariant();
        if (await _users.EmailExistsAsync(normalized, ct)) throw new InvalidOperationException("An account with this email already exists.");
        var user = new AppUser { Name = request.Name.Trim(), Email = email, NormalizedEmail = normalized, Role = "Customer" };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);
        await _users.AddAsync(user, ct);
        return IssueToken(user);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken ct)
    {
        var user = await _users.FindByEmailAsync(request.Email.Trim().ToUpperInvariant(), ct);
        if (user == null || _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed) return null;
        return IssueToken(user);
    }

    public Task<AppUser?> GetProfileAsync(int userId, CancellationToken ct) => _users.FindByIdAsync(userId, ct);

    public async Task<AppUser?> UpdateProfileAsync(int userId, string name, CancellationToken ct)
    {
        var user = await _users.FindByIdAsync(userId, ct);
        if (user == null) return null;
        user.Name = name.Trim();
        await _users.SaveAsync(ct);
        return user;
    }

    private AuthResponseDto IssueToken(AppUser user)
    {
        var issuer = _configuration["jwtConfig:Issuer"]!;
        var audience = _configuration["jwtConfig:Audience"]!;
        var keyText = _configuration["jwtConfig:Key"]!;
        var expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("jwtConfig:TokenValidityMins"));
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.AppUserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyText));
        var token = new JwtSecurityToken(issuer, audience, claims, expires: expires,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new AuthResponseDto { UserId = user.AppUserId, Name = user.Name, Email = user.Email, Role = user.Role,
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token), ExpiresAtUtc = expires };
    }
}
