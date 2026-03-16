using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Entities;

public class HabitLog
{
    [Key]
    public Guid Id { get; set; }
    public Guid HabitId { get; set; }
    public Habit? Habit { get; set; }
    public string UserId { get; set; } = string.Empty;
    public AppUser? User { get; set; }
    public DateTime Date { get; set; }
    public bool IsCompleted { get; set; }
    public decimal? Value { get; set; }
    public string? Note { get; set; }
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
    public string? Mood { get; set; }
    public int? EnergyLevel { get; set; }
}