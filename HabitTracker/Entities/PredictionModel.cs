using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Entities;

public class PredictionModel
{
    [Key]
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public AppUser? User { get; set; }
    public Guid HabitId { get; set; }
    public Habit? Habit { get; set; }
    public string ModelType { get; set; } = "LinearRegression";
    public DateTime TrainedAt { get; set; } = DateTime.UtcNow;
    public double Accuracy { get; set; }
    public DateTime ForecastDate { get; set; }
    public double PredictedCompletionProbability { get; set; }
    public int AvgStreakLength { get; set; }
    public double WeeklyCompletionRate { get; set; }
    public int MissedDaysLastMonth { get; set; }
}