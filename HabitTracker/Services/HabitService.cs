using HabitTracker.Entities;
using HabitTracker.Repositories;

namespace HabitTracker.Services;

public class HabitService : Service<Habit>, IHabitService
{
    private readonly IHabitRepository _habitRepository;

    public HabitService(IHabitRepository habitRepository) : base(habitRepository)
    {
        _habitRepository = habitRepository;
    }

    public async Task<IEnumerable<Habit>> GetByUserIdAsync(string userId)
    {
        return await _habitRepository.GetByUserIdAsync(userId);
    }

    public async Task<Habit?> GetByIdWithLogsAsync(Guid id)
    {
        return await _habitRepository.GetByIdWithLogsAsync(id);
    }

    public async Task<Habit> TrackCompletionAsync(Guid habitId, string userId)
    {
        var habit = await _habitRepository.GetByIdAsync(habitId);
        if (habit == null || habit.UserId != userId)
            throw new InvalidOperationException("Habit not found");

        await _habitRepository.UpdateAsync(habit);
        return habit;
    }

    public async Task<int> GetCurrentStreakAsync(Guid habitId)
    {
        var logs = await _habitRepository.GetByIdWithLogsAsync(habitId);
        if (logs?.Logs == null || !logs.Logs.Any())
            return 0;

        var sortedLogs = logs.Logs.OrderByDescending(l => l.Date).ToList();
        int streak = 1;

        for (int i = 1; i < sortedLogs.Count; i++)
        {
            var diff = (sortedLogs[i - 1].Date - sortedLogs[i].Date).Days;
            if (diff == 1)
                streak++;
            else
                break;
        }

        return streak;
    }
}