using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Entities;

/// <summary>
/// Лог выполнения привычки (история отслеживания)
/// </summary>
public class HabitLog
{
    /// <summary>
    /// Уникальный идентификатор записи в логе
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор связанной привычки
    /// </summary>
    public Guid HabitId { get; set; }

    /// <summary>
    /// Навигационное свойство: связанная привычка
    /// </summary>
    public Habit? Habit { get; set; }

    /// <summary>
    /// Идентификатор пользователя, создавшего запись
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Навигационное свойство: пользователь
    /// </summary>
    public AppUser? User { get; set; }

    /// <summary>
    /// Дата выполнения (или попытки выполнения) привычки
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Флаг: была ли выполнена привычка
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Заметка пользователя о выполнении
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Числовое значение (для числовых привычек)
    /// </summary>
    public decimal? Value { get; set; }

    /// <summary>
    /// Дата и время создания записи в логе
    /// </summary>
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Настроение пользователя в момент выполнения
    /// </summary>
    public string? Mood { get; set; }

    /// <summary>
    /// Уровень энергии пользователя (1-10)
    /// </summary>
    public int? EnergyLevel { get; set; }
}