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
        return await _habitLogRepository.GetByHabitIdAsync(habitId);
    }

    public async Task<IEnumerable<HabitLog>> GetByUserIdAsync(string userId)
    {
        return await _habitLogRepository.GetByUserIdAsync(userId);
    }

    public async Task<HabitLog> LogHabitAsync(Guid habitId, string userId, bool completed, string? notes = null)
    {
        var habitLog = new HabitLog
        {
            Id = Guid.NewGuid(),
            HabitId = habitId,
            Date = DateTime.UtcNow,
            IsCompleted = completed,
            Note = notes              
        };

        return await _habitLogRepository.AddAsync(habitLog);
    }
}