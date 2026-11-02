using BookMyShow.Data;
using BookMyShow.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    public UserRepository(ApplicationDbContext db) => _db = db;
    public Task<AppUser?> FindByEmailAsync(string email, CancellationToken ct) => _db.AppUsers.SingleOrDefaultAsync(x => x.NormalizedEmail == email, ct);
    public Task<AppUser?> FindByIdAsync(int id, CancellationToken ct) => _db.AppUsers.SingleOrDefaultAsync(x => x.AppUserId == id, ct);
    public Task<bool> EmailExistsAsync(string email, CancellationToken ct) => _db.AppUsers.AnyAsync(x => x.NormalizedEmail == email, ct);
    public async Task AddAsync(AppUser user, CancellationToken ct) { _db.AppUsers.Add(user); await _db.SaveChangesAsync(ct); }
    public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
