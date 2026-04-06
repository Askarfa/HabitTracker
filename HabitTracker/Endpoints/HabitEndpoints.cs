using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HabitTracker.Data;
using HabitTracker.Entities;
using HabitTracker.Repositories;

namespace HabitTracker.Endpoints;

public static class HabitEndpoints
{
    public static void MapHabitEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/habits").RequireAuthorization();

        group.MapGet("/", async (
            ClaimsPrincipal user,
            HabitRepository repository) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var habits = await repository.GetAllByUserIdAsync(userId);
            return Results.Ok(habits);
        })
        .WithName("GetAllHabits");

        group.MapGet("/{id:guid}", async (
            Guid id,
            HabitRepository repository) =>
        {
            var habit = await repository.GetByIdAsync(id);
            if (habit == null)
                return Results.NotFound();

            return Results.Ok(habit);
        })
        .WithName("GetHabitById");

        group.MapPost("/", async (
            [FromBody] CreateHabitDto dto,
            ClaimsPrincipal user,
            HabitRepository repository) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var habit = new Habit
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = dto.Name,
                Description = dto.Description,
                Frequency = dto.Frequency,
                TargetStreak = dto.TargetStreak,
                CurrentStreak = 0,
                CreatedAt = DateTime.UtcNow
            };

            await repository.AddAsync(habit);
            return Results.Created($"/api/habits/{habit.Id}", habit);
        })
        .WithName("CreateHabit");

        group.MapPut("/{id:guid}", async (
            Guid id,
            [FromBody] UpdateHabitDto dto,
            HabitRepository repository) =>
        {
            var habit = await repository.GetByIdAsync(id);
            if (habit == null)
                return Results.NotFound();

            habit.Name = dto.Name;
            habit.Description = dto.Description;
            habit.Frequency = dto.Frequency;
            habit.TargetStreak = dto.TargetStreak;

            await repository.UpdateAsync(habit);
            return Results.Ok(habit);
        })
        .WithName("UpdateHabit");

        group.MapDelete("/{id:guid}", async (
            Guid id,
            HabitRepository repository) =>
        {
            var habit = await repository.GetByIdAsync(id);
            if (habit == null)
                return Results.NotFound();

            await repository.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteHabit");
    }
}

public class CreateHabitDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Frequency { get; set; }
    public int TargetStreak { get; set; }
}

public class UpdateHabitDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Frequency { get; set; }
    public int TargetStreak { get; set; }
}