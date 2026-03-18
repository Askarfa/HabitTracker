using HabitTracker.Entities;
using HabitTracker.Repositories;

namespace HabitTracker.Services;

public class GoalService : Service<Goal>, IGoalService
{
    private readonly IGoalRepository _goalRepository;

    public GoalService(IGoalRepository goalRepository) : base(goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task<IEnumerable<Goal>> GetByHabitIdAsync(Guid habitId)
    {
        return await _goalRepository.GetByHabitIdAsync(habitId);
    }

    public async Task<IEnumerable<Goal>> GetByUserIdAsync(string userId)
    {
        return await _goalRepository.GetByUserIdAsync(userId);
    }

    public async Task<Goal> MarkAsCompletedAsync(Guid goalId)
    {
        var goal = await _goalRepository.GetByIdAsync(goalId);
        if (goal == null)
            throw new InvalidOperationException("Goal not found");

        goal.IsCompleted = true;
        goal.CompletedAt = DateTime.UtcNow;

        await _goalRepository.UpdateAsync(goal);
        return goal;
    }
}