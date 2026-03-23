using HabitTracker.Services;
using HabitTracker.Entities;

namespace HabitTracker.Endpoints;

public static class GoalEndpoints
{
    public static void MapGoalEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/goals").WithTags("Goals");

        group.MapGet("/", GetAllGoals);
        group.MapGet("/{id:guid}", GetGoalById);
        group.MapGet("/habit/{habitId:guid}", GetGoalsByHabitId);
        group.MapGet("/user/{userId}", GetGoalsByUserId);
        group.MapPost("/", CreateGoal);
        group.MapPut("/{id:guid}", UpdateGoal);
        group.MapPut("/{id:guid}/complete", MarkGoalAsCompleted);
        group.MapDelete("/{id:guid}", DeleteGoal);
    }

    private static async Task<IResult> GetAllGoals(IGoalService service)
    {
        var goals = await service.GetAllAsync();
        return Results.Ok(goals);
    }

    private static async Task<IResult> GetGoalById(Guid id, IGoalService service)
    {
        var goal = await service.GetByIdAsync(id);
        return goal is null ? Results.NotFound() : Results.Ok(goal);
    }

    private static async Task<IResult> GetGoalsByHabitId(Guid habitId, IGoalService service)
    {
        var goals = await service.GetByHabitIdAsync(habitId);
        return Results.Ok(goals);
    }

    private static async Task<IResult> GetGoalsByUserId(string userId, IGoalService service)
    {
        var goals = await service.GetByUserIdAsync(userId);
        return Results.Ok(goals);
    }

    private static async Task<IResult> CreateGoal(Goal goal, IGoalService service)
    {
        var created = await service.CreateAsync(goal);
        return Results.Created($"/api/goals/{created.Id}", created);
    }

    private static async Task<IResult> UpdateGoal(Guid id, Goal goal, IGoalService service)
    {
        if (!await service.ExistsAsync(id))
            return Results.NotFound();

        goal.Id = id;
        await service.UpdateAsync(goal);
        return Results.NoContent();
    }

    private static async Task<IResult> MarkGoalAsCompleted(Guid id, IGoalService service)
    {
        var completed = await service.MarkAsCompletedAsync(id);
        return Results.Ok(completed);
    }

    private static async Task<IResult> DeleteGoal(Guid id, IGoalService service)
    {
        if (!await service.ExistsAsync(id))
            return Results.NotFound();

        await service.DeleteAsync(id);
        return Results.NoContent();
    }
}