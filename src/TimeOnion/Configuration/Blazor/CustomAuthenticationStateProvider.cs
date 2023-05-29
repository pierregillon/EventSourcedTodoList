using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace TimeOnion.Configuration.Blazor;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomAuthenticationStateProvider(
        ILocalStorageService localStorage,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _localStorage = localStorage;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_httpContextAccessor.IsPrerendering())
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var token = await _localStorage.GetToken();

        var identity = token is not null
            ? new ClaimsIdentity(new JwtSecurityToken(token.Token).Claims, "jwt")
            : new ClaimsIdentity();

        var claimsPrincipal = new ClaimsPrincipal(identity);

        var state = new AuthenticationState(claimsPrincipal);

        return state;
    }
}