using HabitTracker.Entities;

namespace HabitTracker.Services;

public interface IGoalService : IService<Goal>
{
    Task<IEnumerable<Goal>> GetByHabitIdAsync(Guid habitId);
    Task<IEnumerable<Goal>> GetByUserIdAsync(string userId);
    Task<Goal> MarkAsCompletedAsync(Guid goalId);
}