using HabitTracker.Entities;
using HabitTracker.Services;

namespace HabitTracker.Endpoints;

public static class GoalEndpoints
{
    public static void MapGoalEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/goals").WithTags("Goals");

        group.MapGet("/", async (IGoalService service) =>
            await service.GetAllAsync());

        group.MapGet("/{id:guid}", async (Guid id, IGoalService service) =>
        {
            var goal = await service.GetByIdAsync(id);
            return goal is null ? Results.NotFound() : Results.Ok(goal);
        });

        group.MapGet("/habit/{habitId:guid}", async (Guid habitId, IGoalService service) =>
            await service.GetByHabitIdAsync(habitId));

        group.MapGet("/user/{userId}", async (string userId, IGoalService service) =>
            await service.GetByUserIdAsync(userId));

        group.MapPost("/", async (Goal goal, IGoalService service) =>
        {
            var created = await service.CreateAsync(goal);
            return Results.Created($"/api/goals/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", async (Guid id, Goal goal, IGoalService service) =>
        {
            if (!await service.ExistsAsync(id))
                return Results.NotFound();

            goal.Id = id;
            await service.UpdateAsync(goal);
            return Results.NoContent();
        });

        group.MapPut("/{id:guid}/complete", async (Guid id, IGoalService service) =>
        {
            var completed = await service.MarkAsCompletedAsync(id);
            return Results.Ok(completed);
        });

        group.MapDelete("/{id:guid}", async (Guid id, IGoalService service) =>
        {
            if (!await service.ExistsAsync(id))
                return Results.NotFound();

            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}