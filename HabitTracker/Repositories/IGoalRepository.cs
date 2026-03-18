using HabitTracker.Entities;

namespace HabitTracker.Repositories;

public interface IGoalRepository : IRepository<Goal>
{
    Task<IEnumerable<Goal>> GetByHabitIdAsync(Guid habitId);
    Task<IEnumerable<Goal>> GetByUserIdAsync(string userId);
}