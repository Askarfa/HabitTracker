using HabitTracker.Entities;
using HabitTracker.Services;

namespace HabitTracker.Endpoints;

public static class HabitEndpoints
{
    public static void MapHabitEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/habits").WithTags("Habits");

        group.MapGet("/", async (IHabitService service) =>
            await service.GetAllAsync());

        group.MapGet("/{id:guid}", async (Guid id, IHabitService service) =>
        {
            var habit = await service.GetByIdAsync(id);
            return habit is null ? Results.NotFound() : Results.Ok(habit);
        });

        group.MapGet("/user/{userId}", async (string userId, IHabitService service) =>
            await service.GetByUserIdAsync(userId));

        group.MapPost("/", async (Habit habit, IHabitService service) =>
        {
            var created = await service.CreateAsync(habit);
            return Results.Created($"/api/habits/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", async (Guid id, Habit habit, IHabitService service) =>
        {
            if (!await service.ExistsAsync(id))
                return Results.NotFound();

            habit.Id = id;
            await service.UpdateAsync(habit);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IHabitService service) =>
        {
            if (!await service.ExistsAsync(id))
                return Results.NotFound();

            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}