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
        var today = DateTime.UtcNow.Date;
        var habits = await _context.Habits
            .Where(h => h.UserId == userId)
            .Select(h => new Habit
            {
                Id = h.Id,
                UserId = h.UserId,
                Name = h.Name,
                Description = h.Description,
                Frequency = h.Frequency,
                TargetStreak = h.TargetStreak,
                CurrentStreak = h.CurrentStreak,
                BestStreak = h.BestStreak,
                CreatedAt = h.CreatedAt,
                ArchivedAt = h.ArchivedAt,
                LastCompletedAt = h.LastCompletedAt,
                IsCompletedToday = _context.HabitLogs.Any(l => l.HabitId == h.Id && l.Date == today)
            })
            .ToListAsync();
        return habits;
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