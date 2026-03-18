using HabitTracker.Entities;

namespace HabitTracker.Services;

public interface IHabitLogService : IService<HabitLog>
{
    Task<IEnumerable<HabitLog>> GetByHabitIdAsync(Guid habitId);
    Task<IEnumerable<HabitLog>> GetByUserIdAsync(string userId);
    Task<HabitLog> LogHabitAsync(Guid habitId, string userId, bool completed, string? notes = null);
}