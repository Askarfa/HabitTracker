using HabitTracker.Entities;

namespace HabitTracker.Services;

public interface IHabitService : IService<Habit>
{
    Task<IEnumerable<Habit>> GetByUserIdAsync(string userId);
    Task<Habit?> GetByIdWithLogsAsync(Guid id);
    Task<Habit> TrackCompletionAsync(Guid habitId, string userId);
    Task<int> GetCurrentStreakAsync(Guid habitId);
}