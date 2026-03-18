using HabitTracker.Entities;
using HabitTracker.Services;

namespace HabitTracker.Endpoints;

public static class HabitLogEndpoints
{
    public static void MapHabitLogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/habit-logs").WithTags("HabitLogs");

        group.MapGet("/", async (IHabitLogService service) =>
            await service.GetAllAsync());

        group.MapGet("/{id:guid}", async (Guid id, IHabitLogService service) =>
        {
            var log = await service.GetByIdAsync(id);
            return log is null ? Results.NotFound() : Results.Ok(log);
        });

        group.MapGet("/habit/{habitId:guid}", async (Guid habitId, IHabitLogService service) =>
            await service.GetByHabitIdAsync(habitId));

        group.MapGet("/user/{userId}", async (string userId, IHabitLogService service) =>
            await service.GetByUserIdAsync(userId));

        group.MapPost("/", async (HabitLog log, IHabitLogService service) =>
        {
            var created = await service.CreateAsync(log);
            return Results.Created($"/api/habit-logs/{created.Id}", created);
        });

        group.MapPost("/log", async (Guid habitId, string userId, bool completed, string? notes, IHabitLogService service) =>
        {
            var log = await service.LogHabitAsync(habitId, userId, completed, notes);
            return Results.Created($"/api/habit-logs/{log.Id}", log);
        });

        group.MapPut("/{id:guid}", async (Guid id, HabitLog log, IHabitLogService service) =>
        {
            if (!await service.ExistsAsync(id))
                return Results.NotFound();

            log.Id = id;
            await service.UpdateAsync(log);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IHabitLogService service) =>
        {
            if (!await service.ExistsAsync(id))
                return Results.NotFound();

            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}