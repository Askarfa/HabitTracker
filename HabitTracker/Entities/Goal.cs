namespace HabitTracker.Entities;

public class Goal
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime TargetDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid HabitId { get; set; }
    public Habit? Habit { get; set; }
    public string UserId { get; set; } = string.Empty;
    public AppUser? User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}