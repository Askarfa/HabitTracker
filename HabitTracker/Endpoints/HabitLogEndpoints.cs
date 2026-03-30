using HabitTracker.Services;
using HabitTracker.Entities;
using System.Security.Claims;

namespace HabitTracker.Endpoints;

public static class HabitLogEndpoints
{
    public static void MapHabitLogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/habit-logs").WithTags("HabitLogs");

        group.MapGet("/", GetAllHabitLogs);
        group.MapGet("/{id:guid}", GetHabitLogById);
        group.MapGet("/habit/{habitId:guid}", GetLogsByHabitId);
        group.MapGet("/user/{userId}", GetLogsByUserId);
        group.MapPost("/", CreateHabitLog);
        group.MapPost("/log", LogHabit);
        group.MapPost("/{habitId:guid}/complete", MarkHabitAsCompleted); // ✅ НОВЫЙ!
        group.MapPut("/{id:guid}", UpdateHabitLog);
        group.MapDelete("/{id:guid}", DeleteHabitLog);
    }

    private static async Task<IResult> GetAllHabitLogs(IHabitLogService service)
    {
        var logs = await service.GetAllAsync();
        return Results.Ok(logs);
    }

    private static async Task<IResult> GetHabitLogById(Guid id, IHabitLogService service)
    {
        var log = await service.GetByIdAsync(id);
        return log is null ? Results.NotFound() : Results.Ok(log);
    }

    private static async Task<IResult> GetLogsByHabitId(Guid habitId, IHabitLogService service)
    {
        var logs = await service.GetByHabitIdAsync(habitId);
        return Results.Ok(logs);
    }

    private static async Task<IResult> GetLogsByUserId(string userId, IHabitLogService service)
    {
        var logs = await service.GetByUserIdAsync(userId);
        return Results.Ok(logs);
    }

    private static async Task<IResult> CreateHabitLog(HabitLog log, IHabitLogService service)
    {
        var created = await service.CreateAsync(log);
        return Results.Created($"/api/habit-logs/{created.Id}", created);
    }

    private static async Task<IResult> LogHabit(HabitLogRequest request, IHabitLogService service)
    {
        var log = await service.LogHabitAsync(request.HabitId, request.UserId, request.Completed, request.Notes);
        return Results.Created($"/api/habit-logs/{log.Id}", log);
    }

    private static async Task<IResult> MarkHabitAsCompleted(
        Guid habitId,
        IHabitLogService habitLogService,
        ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();

        var log = await habitLogService.LogHabitAsync(habitId, userId, true, null);
        return Results.Ok(log);
    }

    private static async Task<IResult> UpdateHabitLog(Guid id, HabitLog log, IHabitLogService service)
    {
        if (!await service.ExistsAsync(id))
            return Results.NotFound();

        log.Id = id;
        await service.UpdateAsync(log);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteHabitLog(Guid id, IHabitLogService service)
    {
        if (!await service.ExistsAsync(id))
            return Results.NotFound();

        await service.DeleteAsync(id);
        return Results.NoContent();
    }
}

public class HabitLogRequest
{
    public Guid HabitId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public string? Notes { get; set; }
}