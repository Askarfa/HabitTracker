using HabitTracker.Entities;
using HabitTracker.Repositories;

namespace HabitTracker.Services;

public class HabitLogService : Service<HabitLog>, IHabitLogService
{
    private readonly IHabitLogRepository _habitLogRepository;

    public HabitLogService(IHabitLogRepository habitLogRepository) : base(habitLogRepository)
    {
        _habitLogRepository = habitLogRepository;
    }

    public async Task<IEnumerable<HabitLog>> GetByHabitIdAsync(Guid habitId)
    {
        if (habitId == Guid.Empty)
            throw new ArgumentException("Invalid habit ID", nameof(habitId));

        return await _habitLogRepository.GetByHabitIdAsync(habitId);
    }

    public async Task<IEnumerable<HabitLog>> GetByUserIdAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId не может быть пустым", nameof(userId));

        return await _habitLogRepository.GetByUserIdAsync(userId);
    }

    public async Task<HabitLog> LogHabitAsync(Guid habitId, string userId, bool completed, string? notes = null)
    {
        if (habitId == Guid.Empty)
            throw new ArgumentException("Invalid habit ID", nameof(habitId));

        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId не может быть пустым", nameof(userId));

        var habitLog = new HabitLog
        {
            Id = Guid.NewGuid(),
            HabitId = habitId,
            UserId = userId,
            Date = DateTime.UtcNow,
            IsCompleted = completed,
            Note = notes
        };

        return await _habitLogRepository.AddAsync(habitLog);
    }

    public override async Task<HabitLog> CreateAsync(HabitLog log)
    {
        if (log == null)
            throw new ArgumentNullException(nameof(log));

        if (log.HabitId == Guid.Empty)
            throw new ArgumentException("HabitId обязателен", nameof(log.HabitId));

        if (string.IsNullOrWhiteSpace(log.UserId))
            throw new ArgumentException("UserId обязателен", nameof(log.UserId));

        log.Date = DateTime.UtcNow;
        log.LoggedAt = DateTime.UtcNow;

        return await base.CreateAsync(log);
    }

    public override async Task UpdateAsync(HabitLog log)
    {
        if (log == null)
            throw new ArgumentNullException(nameof(log));

        if (!await ExistsAsync(log.Id))
            throw new InvalidOperationException("Лог не найден");

        await base.UpdateAsync(log);
    }
}   