using System.Security.Claims;
using HabitTracker.Services;
using HabitTracker.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Endpoints;

public static class GoalEndpoints
{
    public static void MapGoalEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/goals")
            .WithTags("Goals")
            .RequireAuthorization();

        group.MapGet("/", GetAllGoals);
        group.MapGet("/{id:guid}", GetGoalById);
        group.MapGet("/habit/{habitId:guid}", GetGoalsByHabitId);
        group.MapGet("/user/{userId}", GetGoalsByUserId);
        group.MapPost("/", CreateGoal);
        group.MapPut("/{id:guid}", UpdateGoal);
        group.MapPut("/{id:guid}/complete", MarkGoalAsCompleted);
        group.MapDelete("/{id:guid}", DeleteGoal);
    }

    private static async Task<IResult> GetAllGoals(
        ClaimsPrincipal user,
        IGoalService service)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();

        var goals = await service.GetAllAsync();
        return Results.Ok(goals);
    }

    private static async Task<IResult> GetGoalById(
        Guid id,
        IGoalService service)
    {
        var goal = await service.GetByIdAsync(id);
        return goal is null ? Results.NotFound() : Results.Ok(goal);
    }

    private static async Task<IResult> GetGoalsByHabitId(
        Guid habitId,
        IGoalService service)
    {
        var goals = await service.GetByHabitIdAsync(habitId);
        return Results.Ok(goals);
    }

    private static async Task<IResult> GetGoalsByUserId(
        string userId,
        IGoalService service)
    {
        var goals = await service.GetByUserIdAsync(userId);
        return Results.Ok(goals);
    }

    private static async Task<IResult> CreateGoal(
        [FromBody] CreateGoalDto dto,
        ClaimsPrincipal user,
        IGoalService service)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();

        var goal = new Goal
        {
            UserId = userId,
            Name = dto.Name,             
            Description = dto.Description,
            TargetDate = dto.TargetDate,
            HabitId = dto.HabitId,     
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var created = await service.CreateAsync(goal);
        return Results.Created($"/api/goals/{created.Id}", created);
    }

  
    private static async Task<IResult> UpdateGoal(
        Guid id,
        [FromBody] UpdateGoalDto dto,
        IGoalService service)
    {
        var goal = await service.GetByIdAsync(id);
        if (goal is null)
            return Results.NotFound();

        goal.Name = dto.Name;             
        goal.Description = dto.Description;
        goal.TargetDate = dto.TargetDate;
        goal.HabitId = dto.HabitId;        

        await service.UpdateAsync(goal);
        return Results.NoContent();
    }

    private static async Task<IResult> MarkGoalAsCompleted(
        Guid id,
        IGoalService service)
    {
        var completed = await service.MarkAsCompletedAsync(id);
        return Results.Ok(completed);
    }

    private static async Task<IResult> DeleteGoal(
        Guid id,
        IGoalService service)
    {
        if (!await service.ExistsAsync(id))
            return Results.NotFound();

        await service.DeleteAsync(id);
        return Results.NoContent();
    }
}

public class CreateGoalDto
{
    public string Name { get; set; } = string.Empty;      
    public string? Description { get; set; }
    public DateTime TargetDate { get; set; }
    public Guid HabitId { get; set; }                 
}

public class UpdateGoalDto
{
    public string Name { get; set; } = string.Empty;      
    public string? Description { get; set; }
    public DateTime TargetDate { get; set; }
    public Guid HabitId { get; set; }                      
}