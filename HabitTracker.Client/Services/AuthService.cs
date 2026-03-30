using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace HabitTracker.Client.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private string? _token;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public void SetToken(string token)
    {
        _token = token;
    }

    public string? GetToken()
    {
        return _token;
    }

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);

    public void Logout()
    {
        _token = null;
    }

    public async Task<bool> Login(string email, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", new
            {
                Email = email,
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    SetToken(result.Token);
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка входа: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Register(string email, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/auth/register", new
            {
                Email = email,
                Password = password
            });

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка регистрации: {ex.Message}");
            return false;
        }
    }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}