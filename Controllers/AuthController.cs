using System.Security.Claims;
using BookMyShow.DTOs;
using BookMyShow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [AllowAnonymous, HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request, CancellationToken ct)
    {
        try { return Ok(await _auth.RegisterAsync(request, ct)); }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }

    [AllowAnonymous, HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken ct)
    {
        var result = await _auth.LoginAsync(request, ct);
        return result == null ? Unauthorized("Invalid email or password.") : Ok(result);
    }
}

[ApiController, Authorize]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAuthService _auth;
    public AccountController(IAuthService auth) => _auth = auth;

    [HttpGet("me")]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        var user = await _auth.GetProfileAsync(CurrentUserId(), ct);
        return user == null ? NotFound() : Ok(new { user.AppUserId, user.Name, user.Email, user.Role });
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto request, CancellationToken ct)
    {
        var user = await _auth.UpdateProfileAsync(CurrentUserId(), request.Name, ct);
        return user == null ? NotFound() : Ok(new { user.AppUserId, user.Name, user.Email, user.Role });
    }

    private int CurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
