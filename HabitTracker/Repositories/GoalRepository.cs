using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;
using HabitTracker.Entities;

namespace HabitTracker.Repositories;

public class GoalRepository
{
    private readonly ApplicationDbContext _context;

    public GoalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Goal>> GetAllByUserIdAsync(string userId)
    {
        return await _context.Goals
            .Where(g => g.UserId == userId)
            .ToListAsync();
    }

    public async Task<Goal?> GetByIdAsync(Guid id)
    {
        return await _context.Goals.FindAsync(id);
    }

    public async Task AddAsync(Goal goal)
    {
        await _context.Goals.AddAsync(goal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Goal goal)
    {
        _context.Goals.Update(goal);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var goal = await _context.Goals.FindAsync(id);
        if (goal != null)
        {
            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
        }
    }
}