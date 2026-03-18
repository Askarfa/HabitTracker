using HabitTracker.Data;
using HabitTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Repositories;

public class HabitRepository : Repository<Habit>, IHabitRepository
{
    public HabitRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Habit>> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Where(h => h.UserId == userId && !h.ArchivedAt.HasValue)
            .ToListAsync();
    }

    public async Task<Habit?> GetByIdWithLogsAsync(Guid id)
    {
        return await _dbSet
            .Include(h => h.Logs)
            .Include(h => h.Goals)
            .FirstOrDefaultAsync(h => h.Id == id);
    }
}