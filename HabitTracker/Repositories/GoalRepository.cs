using HabitTracker.Data;
using HabitTracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Repositories;

public class GoalRepository : Repository<Goal>, IGoalRepository
{
    public GoalRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Goal>> GetByHabitIdAsync(Guid habitId)
    {
        return await _dbSet.Where(g => g.HabitId == habitId).ToListAsync();
    }

    public async Task<IEnumerable<Goal>> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(g => g.Habit)
            .Where(g => g.Habit.UserId == userId)
            .ToListAsync();
    }
}