using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;
using HabitTracker.Entities;

namespace HabitTracker.Repositories;

public class HabitLogRepository
{
    private readonly ApplicationDbContext _context;

    public HabitLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HabitLog>> GetByHabitIdAsync(Guid habitId)
    {
        return await _context.HabitLogs
            .Where(hl => hl.HabitId == habitId)
            .ToListAsync();
    }

    public async Task<HabitLog?> GetByHabitIdAndDateAsync(Guid habitId, DateTime date)
    {
        return await _context.HabitLogs
            .FirstOrDefaultAsync(hl => hl.HabitId == habitId
    && hl.Date.Date == date.Date);
    }

    public async Task<HabitLog?> GetByIdAsync(Guid id)
    {
        return await _context.HabitLogs.FindAsync(id);
    }

    public async Task AddAsync(HabitLog log)
    {
        await _context.HabitLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var log = await _context.HabitLogs.FindAsync(id);
        if (log != null)
        {
            _context.HabitLogs.Remove(log);
            await _context.SaveChangesAsync();
        }
    }
}