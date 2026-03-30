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
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание привычки
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Частота выполнения: 0=Ежедневно, 1=Еженедельно, 2=Ежемесячно
    /// </summary>
    public int Frequency { get; set; }

    /// <summary>
    /// Целевое количество дней для серии (streak)
    /// </summary>
    public int TargetStreak { get; set; }

    /// <summary>
    /// Идентификатор владельца привычки
    /// </summary>
    [Required]
    public string? UserId { get; set; }

    /// <summary>
    /// Навигационное свойство: владелец привычки
    /// </summary>
    [JsonIgnore]
    public virtual AppUser? User { get; set; }

    /// <summary>
    /// История выполнений привычки (логи)
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<HabitLog> Logs { get; set; } = new List<HabitLog>();

    /// <summary>
    /// Цели, связанные с этой привычкой
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    /// <summary>
    /// Модели прогнозирования для этой привычки
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<PredictionModel> Predictions { get; set; } = new List<PredictionModel>();

    /// <summary>
    /// Дата и время создания привычки
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата и время архивации привычки (если заархивирована)
    /// </summary>
    public DateTime? ArchivedAt { get; set; }

    /// <summary>
    /// Текущая серия дней выполнения подряд (streak)
    /// </summary>
    public int CurrentStreak { get; set; }

    /// <summary>
    /// Лучшая серия дней выполнения за всё время
    /// </summary>
    public int BestStreak { get; set; }

    /// <summary>
    /// Дата и время последнего выполнения привычки
    /// </summary>
    public DateTime? LastCompletedAt { get; set; }

    /// <summary>
    /// Выполнена ли привычка сегодня (не сохраняется в БД)
    /// </summary>
    [NotMapped]
    public bool IsCompletedToday { get; set; }
}