using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Entities;

/// <summary>
/// Модель для прогнозирования успеха выполнения привычки
/// </summary>
public class PredictionModel
{
    /// <summary>
    /// Уникальный идентификатор прогноза
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор привычки для прогнозирования
    /// </summary>
    public Guid HabitId { get; set; }

    /// <summary>
    /// Навигационное свойство: связанная привычка
    /// </summary>
    public Habit? Habit { get; set; }

    /// <summary>
    /// Дата, на которую сделан прогноз
    /// </summary>
    public DateTime PredictedDate { get; set; }

    /// <summary>
    /// Вероятность успеха выполнения привычки (0-100%)
    /// </summary>
    public decimal SuccessProbability { get; set; }

    /// <summary>
    /// Факторы, влияющие на прогноз
    /// </summary>
    public string? Factors { get; set; }

    /// <summary>
    /// Дата и время создания прогноза
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}