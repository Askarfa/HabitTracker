using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using HabitTracker.Data;
using HabitTracker.Entities;

namespace HabitTracker.Repositories;

public class GoalRepository : IGoalRepository
{
    private readonly ApplicationDbContext _context;

    public GoalRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Goal>> GetAllAsync()
    {
        return await _context.Goals.ToListAsync();
    }

    public async Task<Goal?> GetByIdAsync(Guid id)
    {
        return await _context.Goals.FindAsync(id);
    }

    public async Task<IEnumerable<Goal>> FindAsync(Expression<Func<Goal, bool>> predicate)
    {
        return await _context.Goals.Where(predicate).ToListAsync();
    }

    public async Task<Goal?> FirstOrDefaultAsync(Expression<Func<Goal, bool>> predicate)
    {
        return await _context.Goals.FirstOrDefaultAsync(predicate);
    }

    public async Task<Goal> AddAsync(Goal goal)
    {
        await _context.Goals.AddAsync(goal);
        await _context.SaveChangesAsync();
        return goal;
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

    public async Task DeleteAsync(Goal goal)
    {
        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<Goal, bool>> predicate)
    {
        return await _context.Goals.AnyAsync(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<Goal, bool>> predicate)
    {
        return await _context.Goals.CountAsync(predicate);
    }


    public async Task<Goal> CreateAsync(Goal goal)
    {
        await _context.Goals.AddAsync(goal);
        await _context.SaveChangesAsync();
        return goal;
    }

    public async Task UpdateAsync(Goal goal)
    {
        _context.Goals.Update(goal);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Goal>> GetByHabitIdAsync(Guid habitId)
    {
        return await _context.Goals
            .Where(g => g.HabitId == habitId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Goal>> GetByUserIdAsync(string userId)
    {
        return await _context.Goals
            .Where(g => g.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Goals.AnyAsync(g => g.Id == id);
    }
}