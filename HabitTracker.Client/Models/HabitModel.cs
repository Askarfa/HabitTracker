namespace HabitTracker.Client.Models;

public class HabitModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Frequency { get; set; }
    public int TargetStreak { get; set; }
    public string? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ArchivedAt { get; set; }
    public int CurrentStreak { get; set; }
    public int BestStreak { get; set; }
    public DateTime? LastCompletedAt { get; set; }
    public bool IsCompletedToday { get; set; }
}

public class CreateHabitDto
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public int Frequency { get; set; }
    public int TargetStreak { get; set; }
}

public class UpdateHabitDto
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public int Frequency { get; set; }
    public int TargetStreak { get; set; }
}