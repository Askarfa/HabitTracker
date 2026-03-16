using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Entities;

public class Habit
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public HabitFrequency Frequency { get; set; }
    public HabitType Type { get; set; }
    public int TargetStreak { get; set; }
    public string? ReminderTime { get; set; }
    public string UserId { get; set; } = string.Empty;
    public AppUser? User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ArchivedAt { get; set; }
    public ICollection<HabitLog> Logs { get; set; } = new List<HabitLog>();
    public ICollection<Goal> Goals { get; set; } = new List<Goal>();
}

public enum HabitFrequency { Daily, Weekly, Custom }
public enum HabitType { Positive, Negative, Neutral }