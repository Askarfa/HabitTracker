using HabitTracker.Entities;

namespace HabitTracker.Repositories;

public interface IHabitRepository : IRepository<Habit>
{
    Task<IEnumerable<Habit>> GetByUserIdAsync(string userId);
    Task<Habit?> GetByIdWithLogsAsync(Guid id);
}