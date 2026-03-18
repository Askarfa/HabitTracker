using HabitTracker.Entities;

namespace HabitTracker.Repositories;

public interface IHabitLogRepository : IRepository<HabitLog>
{
    Task<IEnumerable<HabitLog>> GetByHabitIdAsync(Guid habitId);
    Task<IEnumerable<HabitLog>> GetByUserIdAsync(string userId);
    Task<HabitLog?> GetLatestByHabitIdAsync(Guid habitId);
}