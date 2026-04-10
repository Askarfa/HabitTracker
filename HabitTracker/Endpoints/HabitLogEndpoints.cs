using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HabitTracker.Data;
using HabitTracker.Entities;
using HabitTracker.Repositories;

namespace HabitTracker.Endpoints;

public static class HabitLogEndpoints
{
    public static void MapHabitLogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/habit-logs").RequireAuthorization();

        group.MapGet("/habit/{habitId:guid}", async (
            Guid habitId,
            ClaimsPrincipal user,
            HabitLogRepository repository) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var logs = await repository.GetByHabitIdAsync(habitId);
            return Results.Ok(logs);
        })
        .WithName("GetHabitLogs");

        group.MapPost("/{habitId:guid}/complete", async (
            Guid habitId,
            ClaimsPrincipal user,
            UserManager<AppUser> userManager,
            HabitRepository habitRepository,
            HabitLogRepository logRepository) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var habit = await habitRepository.GetByIdAsync(habitId);
            if (habit == null)
                return Results.NotFound();

            var today = DateTime.UtcNow.Date;

            var existingLog = await logRepository.GetByHabitIdAndDateAsync(habitId, today);
            if (existingLog != null)
                return Results.BadRequest(new { message = "Привычка уже отмечена сегодня" });

            var log = new HabitLog
            {
                Id = Guid.NewGuid(),
                HabitId = habitId,
                UserId = userId,
                Date = DateTime.UtcNow.Date,
                IsCompleted = true,
                Note = "Выполнено!",
                LoggedAt = DateTime.UtcNow
            };

            await logRepository.AddAsync(log);

            habit.CurrentStreak++;
            await habitRepository.UpdateAsync(habit);

            return Results.Ok(new
            {
                habit.Id,
                habit.Name,
                habit.CurrentStreak,
                habit.TargetStreak,
                message = "Привычка отмечена как выполненная!"
            });
        })
        .WithName("CompleteHabit");

        group.MapDelete("/{habitId:guid}/cancel", async (
    Guid habitId,
    ClaimsPrincipal user,
    HabitRepository habitRepository,
    HabitLogRepository logRepository) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var habit = await habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
                return Results.NotFound();

            var today = DateTime.UtcNow.Date;
            var log = await logRepository.GetByHabitIdAndDateAsync(habitId, today);
            if (log == null)
                return Results.BadRequest(new { message = "Нет отметки за сегодня" });

            await logRepository.DeleteAsync(log.Id);

            if (habit.CurrentStreak > 0)
                habit.CurrentStreak--;
            await habitRepository.UpdateAsync(habit);

            return Results.Ok(new { message = "Выполнение отменено", habit.CurrentStreak });
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            HabitLogRepository repository) =>
        {
            var log = await repository.GetByIdAsync(id);
            if (log == null)
                return Results.NotFound();

            await repository.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteHabitLog");
    }
}