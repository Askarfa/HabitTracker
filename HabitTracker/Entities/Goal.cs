using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Entities;

public class Goal
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid HabitId { get; set; }
    public Habit? Habit { get; set; }
    public GoalType Type { get; set; }
    public decimal TargetValue { get; set; }
    public decimal CurrentValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsAchieved { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum GoalType { Streak, Count, Value }