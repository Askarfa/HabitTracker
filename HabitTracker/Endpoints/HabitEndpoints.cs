using HabitTracker.Services;
using HabitTracker.Entities;

namespace HabitTracker.Endpoints;

public static class HabitEndpoints
{
    public static void MapHabitEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/habits").WithTags("Habits");

        group.MapGet("/", GetAllHabits);
        group.MapGet("/{id:guid}", GetHabitById);
        group.MapGet("/user/{userId}", GetHabitsByUserId);
        group.MapPost("/", CreateHabit);
        group.MapPut("/{id:guid}", UpdateHabit);
        group.MapDelete("/{id:guid}", DeleteHabit);
    }

    private static async Task<IResult> GetAllHabits(IHabitService service)
    {
        var habits = await service.GetAllAsync();
        return Results.Ok(habits);
    }

    private static async Task<IResult> GetHabitById(Guid id, IHabitService service)
    {
        var habit = await service.GetByIdAsync(id);
        return habit is null ? Results.NotFound() : Results.Ok(habit);
    }

    private static async Task<IResult> GetHabitsByUserId(string userId, IHabitService service)
    {
        var habits = await service.GetByUserIdAsync(userId);
        return Results.Ok(habits);
    }

    private static async Task<IResult> CreateHabit(Habit habit, IHabitService service)
    {
        var created = await service.CreateAsync(habit);
        return Results.Created($"/api/habits/{created.Id}", created);
    }

    private static async Task<IResult> UpdateHabit(Guid id, Habit habit, IHabitService service)
    {
        if (!await service.ExistsAsync(id))
            return Results.NotFound();

        habit.Id = id;
        await service.UpdateAsync(habit);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteHabit(Guid id, IHabitService service)
    {
        if (!await service.ExistsAsync(id))
            return Results.NotFound();

        await service.DeleteAsync(id);
        return Results.NoContent();
    }
}