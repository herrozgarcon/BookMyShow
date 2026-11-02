using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Models;

public class AppUser
{
    [Key]
    public int AppUserId { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, MaxLength(150)] public string NormalizedEmail { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    [Required] public string Role { get; set; } = "Customer";
}
