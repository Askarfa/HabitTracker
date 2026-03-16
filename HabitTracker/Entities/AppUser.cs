using Microsoft.AspNetCore.Identity;
using HabitTracker.Entities;

namespace HabitTracker.Entities;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public string? TimeZone { get; set; } = "UTC";

    public ICollection<Habit> Habits { get; set; } = new List<Habit>();
    public ICollection<HabitLog> HabitLogs { get; set; } = new List<HabitLog>();
    public ICollection<PredictionModel> Predictions { get; set; } = new List<PredictionModel>();
}