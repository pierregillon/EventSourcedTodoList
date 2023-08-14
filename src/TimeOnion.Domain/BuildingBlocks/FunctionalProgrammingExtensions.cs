namespace TimeOnion.Domain.BuildingBlocks;

public static class FunctionalProgrammingExtensions
{
    public static TResult Pipe<T, TResult>(this T element, Func<T, TResult> projection) => projection(element);

    public static TResult MapIf<T, TResult>(
        this T element,
        Predicate<T> predicate,
        Func<T, TResult> mapIfTrue,
        Func<T, TResult> mapIfFalse
    ) => predicate(element) ? mapIfTrue(element) : mapIfFalse(element);

    public static TResult? MapIfNotNull<T, TResult>(
        this T? element,
        Func<T, TResult> mapIfTrue
    ) => element is null ? (TResult?)(object?)null : mapIfTrue(element);

    public static T If<T>(
        this T element,
        Predicate<T> predicate,
        T ifTrue
    ) => predicate(element) ? ifTrue : element;

    public static T As<T>(this object element)
    {
        if (element is T t)
        {
            return t;
        }

        throw new InvalidOperationException($"'{element.GetType().Name}' cannot be cast as '{typeof(T).Name}'");
    }
}