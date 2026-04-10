using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace HabitTracker.Client.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private string? _token;

    public AuthService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");

            // ← ДОБАВЬТЕ ЭТО: устанавливаем токен при инициализации!
            if (!string.IsNullOrEmpty(_token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);
            }
        }
        catch { _token = null; }
        OnAuthStateChanged?.Invoke();
    }

    public async Task SetToken(string token)
    {
        _token = token;
        await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);

        // ← ДОБАВЬТЕ ЭТО: добавляем токен к запросам!
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        OnAuthStateChanged?.Invoke();
    }

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
    public string? GetToken() => _token;
    public event Action? OnAuthStateChanged;

    public async Task Logout()
    {
        _token = null;
        await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");

        // ← ДОБАВЬТЕ ЭТО: убираем токен из запросов!
        _http.DefaultRequestHeaders.Authorization = null;

        OnAuthStateChanged?.Invoke();
    }

    public async Task<(bool Success, string? Error)> Login(string email, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", new { email, password });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result?.Token != null)
                {
                    await SetToken(result.Token);  // ← Здесь вызывается SetToken с заголовком!
                    return (true, null);
                }
            }
            return (false, "Неверный email или пароль");
        }
        catch
        {
            return (false, "Ошибка сети");
        }
    }

    public async Task<(bool Success, string? Error)> Register(string email, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/auth/register", new { email, password });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result?.Token != null)
                {
                    await SetToken(result.Token);  // ← Здесь вызывается SetToken с заголовком!
                    return (true, null);
                }
            }
            return (false, "Ошибка регистрации");
        }
        catch
        {
            return (false, "Ошибка сети");
        }
    }
}

public class LoginResponse
{
    public string Token { get; set; } = "";
    public string Email { get; set; } = "";
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
}