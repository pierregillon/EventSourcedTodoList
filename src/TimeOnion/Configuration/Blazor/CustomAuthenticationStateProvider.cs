using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TimeOnion.Configuration.Authentication;
using TimeOnion.Configuration.GlobalContext;
using TimeOnion.Domain;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Configuration.Blazor;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly InMemoryUserContextProvider _userContextProvider;
    private static readonly ClaimsIdentity Anonymous = new();

    public CustomAuthenticationStateProvider(
        ILocalStorageService localStorage,
        IHttpContextAccessor httpContextAccessor,
        IQueryDispatcher queryDispatcher,
        InMemoryUserContextProvider userContextProvider
    )
    {
        _localStorage = localStorage;
        _httpContextAccessor = httpContextAccessor;
        _queryDispatcher = queryDispatcher;
        _userContextProvider = userContextProvider;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var claims = await GetClaims();

        return new AuthenticationState(new ClaimsPrincipal(claims));
    }

    private async Task<ClaimsIdentity> GetClaims()
    {
        if (_httpContextAccessor.IsPrerendering())
        {
            return Anonymous;
        }

        var token = await _localStorage.GetToken();

        var claims = await ParseClaims(token);

        UpdateContextProvider(claims);

        return claims;
    }

    private async Task<ClaimsIdentity> ParseClaims(JwtToken? token)
    {
        if (token is null || token.ValidTo < DateTime.UtcNow)
        {
            return Anonymous;
        }

        var identity = new ClaimsIdentity(new JwtSecurityToken(token.Token).Claims, "jwt");

        return await IsIdentityValid(identity) ? identity : Anonymous;
    }

    private async Task<bool> IsIdentityValid(ClaimsIdentity identity)
    {
        var userId = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

        return await _queryDispatcher.Dispatch(new UserExistsQuery(UserId.Parse(userId)));
    }

    private void UpdateContextProvider(ClaimsIdentity identity)
    {
        var userId = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var userEmail = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        if (userId is null || userEmail is null)
        {
            _userContextProvider.RemoveUserContext();
        }
        else
        {
            _userContextProvider.SetUserContext(new UserContext(UserId.Parse(userId), new EmailAddress(userEmail)));
        }
    }
}