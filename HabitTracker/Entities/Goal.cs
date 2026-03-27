using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Entities;

/// <summary>
/// Цель, связанная с привычкой
/// </summary>
public class Goal
{
    /// <summary>
    /// Уникальный идентификатор цели
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Название цели (обязательное поле)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание цели
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Целевая дата достижения цели
    /// </summary>
    public DateTime TargetDate { get; set; }

    /// <summary>
    /// Флаг: достигнута ли цель
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>
    /// Дата и время завершения цели
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Идентификатор связанной привычки
    /// </summary>
    public Guid HabitId { get; set; }

    /// <summary>
    /// Навигационное свойство: связанная привычка
    /// </summary>
    public Habit? Habit { get; set; }

    /// <summary>
    /// Идентификатор владельца цели
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Навигационное свойство: владелец цели
    /// </summary>
    public AppUser? User { get; set; }

    /// <summary>
    /// Дата и время создания цели
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}