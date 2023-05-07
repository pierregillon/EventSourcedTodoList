namespace TimeOnion.Domain.Todo.Core;

public static class ListExtensions
{
    public static IList<T> Replace<T>(this IList<T> list, T existingElement, T newElement)
    {
        var indexOfExistingElement = list.IndexOf(existingElement);
        if (indexOfExistingElement == -1)
        {
            throw new InvalidOperationException("The existing element does not belong to the list");
        }

        list.Insert(indexOfExistingElement, newElement);
        list.Remove(existingElement);

        return list;
    }

    public static IEnumerable<T> Replace<T>(this IEnumerable<T> enumerable, T existingElement, T newElement)
    {
        foreach (var element in enumerable)
        {
            if (Equals(element, existingElement))
            {
                yield return newElement;
            }
            else
            {
                yield return element;
            }
        }
    }

    public static IEnumerable<T> InsertAt<T>(this IEnumerable<T> enumerable, T newElement, int index)
    {
        using var enumerator = enumerable.GetEnumerator();

        var currentIndex = 0;

        while (enumerator.MoveNext())
        {
            if (currentIndex == index)
            {
                yield return newElement;
            }

            yield return enumerator.Current;

            currentIndex++;
        }

        if (currentIndex == index)
        {
            yield return newElement;
        }
    }

    public static IEnumerable<T> InsertAfter<T>(this IEnumerable<T> enumerable, T previousElement, T newElement)
    {
        foreach (var element in enumerable)
        {
            yield return element;

            if (Equals(element, previousElement))
            {
                yield return newElement;
            }
        }
    }
}