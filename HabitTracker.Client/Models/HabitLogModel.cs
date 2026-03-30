namespace HabitTracker.Client.Models;

public class HabitLogModel
{
    public Guid Id { get; set; }
    public Guid HabitId { get; set; }
    public DateTime Date { get; set; }
    public bool IsCompleted { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}