using System.Net.Http.Json;
using HabitTracker.Client.Models;

namespace HabitTracker.Client.Services;

public class HabitService
{
    private readonly HttpClient _http;
    public HabitService(HttpClient http) => _http = http;

    public async Task<List<HabitModel>> GetHabitsAsync()
        => await _http.GetFromJsonAsync<List<HabitModel>>("/api/habits") ?? new();

    public async Task<HabitModel?> GetHabitAsync(Guid id)
        => await _http.GetFromJsonAsync<HabitModel>($"/api/habits/{id}");

    public async Task<HabitModel?> CreateHabitAsync(CreateHabitDto dto)
    {
        var response = await _http.PostAsJsonAsync("/api/habits", dto);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<HabitModel>() : null;
    }

    public async Task<bool> UpdateHabitAsync(Guid id, UpdateHabitDto dto)
    {
        var response = await _http.PutAsJsonAsync($"/api/habits/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteHabitAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"/api/habits/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<(bool Success, string? Error)> CompleteHabitAsync(Guid habitId)
    {
        var response = await _http.PostAsync($"/api/habit-logs/{habitId}/complete", null);
        return response.IsSuccessStatusCode ? (true, null) : (false, "Ошибка");
    }
}