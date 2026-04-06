using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;
using HabitTracker.Entities;

namespace HabitTracker.Repositories;

public class HabitRepository
{
    private readonly ApplicationDbContext _context;

    public HabitRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Habit>> GetAllByUserIdAsync(string userId)
    {
        return await _context.Habits
            .Where(h => h.UserId == userId)
            .ToListAsync();
    }

    public async Task<Habit?> GetByIdAsync(Guid id)
    {
        return await _context.Habits.FindAsync(id);
    }

    public async Task AddAsync(Habit habit)
    {
        await _context.Habits.AddAsync(habit);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Habit habit)
    {
        _context.Habits.Update(habit);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var habit = await _context.Habits.FindAsync(id);
        if (habit != null)
        {
            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync();
        }
    }
}