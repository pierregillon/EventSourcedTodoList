namespace TimeOnion.Configuration;

public static class HttpContextAccessorExtensions
{
    public static bool IsPrerendering(this IHttpContextAccessor httpContextAccessor) =>
        httpContextAccessor.HttpContext?.Response.HasStarted == false;
}