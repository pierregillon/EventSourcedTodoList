using AngleSharp.Dom;
using Bunit;

namespace TimeOnion.WebApp.Tests.Unit;

public static class ElementExtensions
{
    public static async Task LongClick(this IElement element, int delayInMilliseconds = 600)
    {
        element.MouseDown();
        await Task.Delay(delayInMilliseconds);
        element.MouseUp();
    }
}