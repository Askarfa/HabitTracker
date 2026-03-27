using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HabitTracker.Entities;

/// <summary>
/// Привычка пользователя для отслеживания
/// </summary>
public class Habit
{
    /// <summary>
    /// Уникальный идентификатор привычки
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Название привычки (обязательное поле)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание привычки
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Частота выполнения: 0=Ежедневно, 1=Еженедельно, 2=Ежемесячно
    /// </summary>
    public int Frequency { get; set; }

    /// <summary>
    /// Тип привычки: 0=Бинарная, 1=Числовая, 2=Текстовая
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// Целевое количество дней для серии (streak)
    /// </summary>
    public int TargetStreak { get; set; }

    /// <summary>
    /// Время напоминания о выполнении привычки
    /// </summary>
    public string? ReminderTime { get; set; }

    /// <summary>
    /// Идентификатор владельца привычки (может быть null)
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Навигационное свойство: владелец привычки
    /// </summary>
    [JsonIgnore]
    public AppUser? User { get; set; }

    /// <summary>
    /// История выполнений привычки (логи)
    /// </summary>
    [JsonIgnore]
    public ICollection<HabitLog> Logs { get; set; } = new List<HabitLog>();

    /// <summary>
    /// Цели, связанные с этой привычкой
    /// </summary>
    [JsonIgnore]
    public ICollection<Goal> Goals { get; set; } = new List<Goal>();

    /// <summary>
    /// Модели прогнозирования для этой привычки
    /// </summary>
    [JsonIgnore]
    public ICollection<PredictionModel> Predictions { get; set; } = new List<PredictionModel>();

    /// <summary>
    /// Дата и время создания привычки
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата и время архивации привычки (если заархивирована)
    /// </summary>
    public DateTime? ArchivedAt { get; set; }
}