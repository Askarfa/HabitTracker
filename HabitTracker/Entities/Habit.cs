using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Entities;

public class Habit
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Frequency { get; set; } // 0 = Daily, 1 = Weekly, 2 = Monthly

    public int Type { get; set; } // 0 = Binary, 1 = Numeric, 2 = Text

    public int TargetStreak { get; set; }

    public TimeSpan? ReminderTime { get; set; }

    public string UserId { get; set; } = string.Empty;

    public AppUser? User { get; set; }

    public ICollection<HabitLog> Logs { get; set; } = new List<HabitLog>();

    public ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ArchivedAt { get; set; }
}
