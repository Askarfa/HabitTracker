using Microsoft.AspNetCore.Identity;
using HabitTracker.Entities;

namespace HabitTracker.Entities;

/// <summary>
/// Пользователь системы
/// </summary>
public class AppUser : IdentityUser
{
    /// <summary>
    /// Отображаемое имя пользователя
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Дата и время регистрации пользователя
    /// </summary>
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Часовой пояс пользователя (по умолчанию UTC)
    /// </summary>
    public string? TimeZone { get; set; } = "UTC";

    /// <summary>
    /// Список привычек пользователя
    /// </summary>
    public ICollection<Habit> Habits { get; set; } = new List<Habit>();

    /// <summary>
    /// История выполнения привычек (логи)
    /// </summary>
    public ICollection<HabitLog> HabitLogs { get; set; } = new List<HabitLog>();

    /// <summary>
    /// Цели пользователя
    /// </summary>
    public ICollection<Goal> Goals { get; set; } = new List<Goal>();

    /// <summary>
    /// Модели прогнозирования для привычек
    /// </summary>
    public ICollection<PredictionModel> Predictions { get; set; } = new List<PredictionModel>();
}