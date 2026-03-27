using HabitTracker.Repositories;
using HabitTracker.Entities;

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
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId не может быть пустым", nameof(userId));

        return await _habitRepository.GetByUserIdAsync(userId);
    }

    public async Task<Habit?> GetByIdWithLogsAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid habit ID", nameof(id));

        return await _habitRepository.GetByIdWithLogsAsync(id);
    }

    public async Task<Habit> TrackCompletionAsync(Guid habitId, string userId)
    {
        if (habitId == Guid.Empty)
            throw new ArgumentException("Invalid habit ID", nameof(habitId));

        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId не может быть пустым", nameof(userId));

        var habit = await _habitRepository.GetByIdAsync(habitId);
        if (habit == null)
            throw new InvalidOperationException("Привычка не найдена");

        if (habit.UserId != userId)
            throw new UnauthorizedAccessException("Доступ запрещён");

        await _habitRepository.UpdateAsync(habit);
        return habit;
    }

    public async Task<int> GetCurrentStreakAsync(Guid habitId)
    {
        if (habitId == Guid.Empty)
            throw new ArgumentException("Invalid habit ID", nameof(habitId));

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

    public override async Task<Habit> CreateAsync(Habit habit)
    {
        if (habit == null)
            throw new ArgumentNullException(nameof(habit));

        if (string.IsNullOrWhiteSpace(habit.Name))
            throw new ArgumentException("Название привычки обязательно", nameof(habit.Name));

        if (habit.Name.Length > 100)
            throw new ArgumentException("Название не должно превышать 100 символов", nameof(habit.Name));

        if (habit.TargetStreak < 0)
            throw new ArgumentException("TargetStreak не может быть отрицательным", nameof(habit.TargetStreak));

        habit.CreatedAt = DateTime.UtcNow;
        return await base.CreateAsync(habit);
    }

    public override async Task UpdateAsync(Habit habit)
    {
        if (habit == null)
            throw new ArgumentNullException(nameof(habit));

        if (!await ExistsAsync(habit.Id))
            throw new InvalidOperationException("Привычка не найдена");

        if (string.IsNullOrWhiteSpace(habit.Name))
            throw new ArgumentException("Название привычки обязательно", nameof(habit.Name));

        await base.UpdateAsync(habit);
    }
}