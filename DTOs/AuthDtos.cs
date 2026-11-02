using System.ComponentModel.DataAnnotations;

namespace BookMyShow.DTOs;

public class RegisterRequestDto
{
    [Required, StringLength(100, MinimumLength = 1)] public string Name { get; set; } = "";
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = "";
    [Required, StringLength(100, MinimumLength = 8)] public string Password { get; set; } = "";
}

public class LoginRequestDto
{
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required] public string Password { get; set; } = "";
}

public class AuthResponseDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public string AccessToken { get; set; } = "";
    public DateTime ExpiresAtUtc { get; set; }
}

public class UpdateProfileDto
{
    [Required, StringLength(100, MinimumLength = 1)] public string Name { get; set; } = "";
}
