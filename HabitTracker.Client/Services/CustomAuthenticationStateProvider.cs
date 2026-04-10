using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace HabitTracker.Client.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthService _authService;
    private AuthenticationState? _cachedState;
    private bool _initialized = false;

    public CustomAuthenticationStateProvider(AuthService authService)
    {
        _authService = authService;
        _authService.OnAuthStateChanged += async () =>
        {
            _cachedState = null;
            var state = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        };
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_cachedState != null)
            return _cachedState;

        if (!_initialized)
        {
            await _authService.InitializeAsync();
            _initialized = true;
        }

        var token = _authService.GetToken();
        if (string.IsNullOrEmpty(token))
        {
            _cachedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            return _cachedState;
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "User"),
            new Claim(ClaimTypes.Authentication, token)
        }, "jwt");

        var user = new ClaimsPrincipal(identity);
        _cachedState = new AuthenticationState(user);
        return _cachedState;
    }
}