using HabitTracker.Data;
using HabitTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Repositories;

public class AppUserRepository : Repository<AppUser>, IAppUserRepository
{
    public AppUserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<AppUser?> GetByIdWithHabitsAsync(string id)
    {
        return await _dbSet
            .Include(u => u.Habits)
            .Include(u => u.Goals)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}