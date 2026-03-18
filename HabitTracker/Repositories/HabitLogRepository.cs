using HabitTracker.Data;
using HabitTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Repositories;

public class HabitLogRepository : Repository<HabitLog>, IHabitLogRepository
{
    public HabitLogRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<HabitLog>> GetByHabitIdAsync(Guid habitId)
    {
        return await _dbSet
            .Where(l => l.HabitId == habitId)
            .OrderByDescending(l => l.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<HabitLog>> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(l => l.Habit)
            .Where(l => l.Habit.UserId == userId)
            .OrderByDescending(l => l.Date)
            .ToListAsync();
    }

    public async Task<HabitLog?> GetLatestByHabitIdAsync(Guid habitId)
    {
        return await _dbSet
            .Where(l => l.HabitId == habitId)
            .OrderByDescending(l => l.Date)
            .FirstOrDefaultAsync();
    }
}