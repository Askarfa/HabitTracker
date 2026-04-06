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

    // ← Асинхронная инициализация — загружаем токен из localStorage
    public async Task InitializeAsync()
    {
        try
        {
            _token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (!string.IsNullOrEmpty(_token))
            {
                Console.WriteLine("✅ Токен загружен из localStorage");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка загрузки токена: {ex.Message}");
            _token = null;
        }
    }

    public void SetToken(string token)
    {
        _token = token;

        // Сохраняем в localStorage
        _ = _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);

        Console.WriteLine("✅ Токен сохранён");

        // Уведомляем компоненты об изменении
        OnAuthStateChanged?.Invoke();
    }

    public string? GetToken()
    {
        return _token;
    }

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);

    // ← Событие для уведомления компонентов об изменении авторизации
    public event Action? OnAuthStateChanged;

    public void Logout()
    {
        _token = null;

        _ = _js.InvokeVoidAsync("localStorage.removeItem", "authToken");

        Console.WriteLine("🚪 Пользователь вышел");

        OnAuthStateChanged?.Invoke();
    }

    public async Task<bool> Login(string email, string password)
    {
        try
        {
            Console.WriteLine($"🔐 Вход: {email}");

            var response = await _http.PostAsJsonAsync("/api/auth/login", new
            {
                Email = email.Trim(),
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    SetToken(result.Token);
                    Console.WriteLine("✅ Вход успешен");
                    return true;
                }
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Ошибка входа: {error}");
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
            return false;
        }
    }

    public async Task<LoginResponse?> Register(string email, string password)
    {
        try
        {
            Console.WriteLine($"📝 Регистрация: {email}");

            var response = await _http.PostAsJsonAsync("/api/auth/register", new
            {
                Email = email.Trim(),
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    SetToken(result.Token);
                    Console.WriteLine("✅ Регистрация успешна");
                }

                return result;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Ошибка регистрации: {error}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
            return null;
        }
    }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}