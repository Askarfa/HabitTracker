using System.Security.Claims;
using HabitTracker.Entities;
using HabitTracker.Services;

namespace HabitTracker.Endpoints;

public static class HabitEndpoints
{
    public static void MapHabitEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/habits").WithTags("Habits");

        group.MapGet("/", async (
            IHabitService service,
            IHabitLogService logService,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var habits = await service.GetByUserIdAsync(userId);

            foreach (var habit in habits)
            {
                var logs = await logService.GetByHabitIdAsync(habit.Id);
                var today = DateTime.UtcNow.Date;

                habit.IsCompletedToday = logs.Any(l =>
                    l.Date.Date == today && l.IsCompleted);

                habit.CurrentStreak = CalculateCurrentStreak(logs);
                habit.LastCompletedAt = logs
                    .Where(l => l.IsCompleted)
                    .OrderByDescending(l => l.Date)
                    .FirstOrDefault()?.Date;
            }

            return Results.Ok(habits);
        })
        .RequireAuthorization()
        .WithName("GetHabits");

        group.MapGet("/{id}", async (Guid id, IHabitService service) =>
        {
            var habit = await service.GetByIdAsync(id);
            return habit is null ? Results.NotFound() : Results.Ok(habit);
        })
        .WithName("GetHabitById");

        group.MapPost("/", async (Habit habit, IHabitService service, ClaimsPrincipal user) =>
        {
            if (user.Identity?.IsAuthenticated == true)
            {
                habit.UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            var created = await service.CreateAsync(habit);
            return Results.Created($"/api/habits/{created.Id}", created);
        })
        .RequireAuthorization()
        .WithName("CreateHabit");

        group.MapPut("/{id}", async (Guid id, Habit updatedHabit, IHabitService service) =>
        {
            var habit = await service.GetByIdAsync(id);
            if (habit is null)
                return Results.NotFound();

            habit.Name = updatedHabit.Name;
            habit.Description = updatedHabit.Description;
            habit.Frequency = updatedHabit.Frequency;
            habit.TargetStreak = updatedHabit.TargetStreak;

            await service.UpdateAsync(habit);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("UpdateHabit");

        group.MapDelete("/{id}", async (Guid id, IHabitService service) =>
        {
            var habit = await service.GetByIdAsync(id);
            if (habit is null)
                return Results.NotFound();

            await service.DeleteAsync(id);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteHabit");
    }

    private static int CalculateCurrentStreak(IEnumerable<HabitLog> logs)
    {
        var completedLogs = logs
            .Where(l => l.IsCompleted)
            .OrderByDescending(l => l.Date.Date)
            .ToList();

        if (!completedLogs.Any()) return 0;

        int streak = 0;
        var currentDate = DateTime.UtcNow.Date;

        foreach (var log in completedLogs)
        {
            if (log.Date.Date == currentDate)
            {
                streak++;
                currentDate = currentDate.AddDays(-1);
            }
            else if (log.Date.Date == currentDate)
            {
                currentDate = currentDate.AddDays(-1);
            }
            else
            {
                break;
            }
        }

        return streak;
    }
}