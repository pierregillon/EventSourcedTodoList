using Blazored.LocalStorage;
using TimeOnion.Configuration.Authentication;

namespace TimeOnion.Configuration.Blazor;

public static class LocalStorageExtensions
{
    private const string TokenFieldName = "token";

    public static async Task SetToken(this ILocalStorageService localStorage, JwtToken token) =>
        await localStorage.SetItemAsync(TokenFieldName, token);

    public static async Task RemoveToken(this ILocalStorageService localStorage) =>
        await localStorage.RemoveItemAsync(TokenFieldName);

    public static async Task<JwtToken?> GetToken(this ILocalStorageService localStorage) =>
        await localStorage.GetItemAsync<JwtToken?>(TokenFieldName);
}