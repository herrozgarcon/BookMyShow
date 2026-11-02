using BookMyShow.Models;

namespace BookMyShow.Repositories;

public interface IUserRepository
{
    Task<AppUser?> FindByEmailAsync(string normalizedEmail, CancellationToken ct);
    Task<AppUser?> FindByIdAsync(int userId, CancellationToken ct);
    Task<bool> EmailExistsAsync(string normalizedEmail, CancellationToken ct);
    Task AddAsync(AppUser user, CancellationToken ct);
    Task SaveAsync(CancellationToken ct);
}
