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
        if (habitId == Guid.Empty)
            throw new ArgumentException("Invalid habit ID", nameof(habitId));

        return await _goalRepository.GetByHabitIdAsync(habitId);
    }

    public async Task<IEnumerable<Goal>> GetByUserIdAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId не может быть пустым", nameof(userId));

        return await _goalRepository.GetByUserIdAsync(userId);
    }

    public async Task<Goal> MarkAsCompletedAsync(Guid goalId)
    {
        if (goalId == Guid.Empty)
            throw new ArgumentException("Invalid goal ID", nameof(goalId));

        var goal = await _goalRepository.GetByIdAsync(goalId);
        if (goal == null)
            throw new InvalidOperationException("Цель не найдена");

        goal.IsCompleted = true;
        goal.CompletedAt = DateTime.UtcNow;

        await _goalRepository.UpdateAsync(goal);
        return goal;
    }

    public override async Task<Goal> CreateAsync(Goal goal)
    {
        if (goal == null)
            throw new ArgumentNullException(nameof(goal));

        if (string.IsNullOrWhiteSpace(goal.Name))
            throw new ArgumentException("Название цели обязательно", nameof(goal.Name));

        if (goal.TargetDate < DateTime.UtcNow)
            throw new ArgumentException("Целевая дата должна быть в будущем", nameof(goal.TargetDate));

        goal.CreatedAt = DateTime.UtcNow;
        return await base.CreateAsync(goal);
    }

    public override async Task UpdateAsync(Goal goal)
    {
        if (goal == null)
            throw new ArgumentNullException(nameof(goal));

        if (!await ExistsAsync(goal.Id))
            throw new InvalidOperationException("Цель не найдена");

        if (string.IsNullOrWhiteSpace(goal.Name))
            throw new ArgumentException("Название цели обязательно", nameof(goal.Name));

        await base.UpdateAsync(goal);
    }
}