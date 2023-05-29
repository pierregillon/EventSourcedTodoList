using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Components;

namespace TimeOnion.Configuration.Blazor;

public static class ExtensionMethods
{
    public static NameValueCollection QueryString(this NavigationManager navigationManager)
    {
        var query = new Uri(navigationManager.Uri).Query;
        return HttpUtility.ParseQueryString(query);
    }

    public static string QueryString(this NavigationManager navigationManager, string key) =>
        navigationManager.QueryString()[key] ?? string.Empty;
}