namespace TimeOnion.Domain;

public static class StringExtensions
{
    public static string ToAcronym(this string value, int count = 2)
    {
        var words = SplitIntoWords(value);

        if (words.Count == 1)
        {
            var uniqueWord = words.Single();
            return uniqueWord.ToUpper()[..Math.Min(uniqueWord.Length, count)];
        }

        var firstLetterOfEachWord = words
            .Take(count)
            .Select(x => char.ToUpper(x.First()))
            .ToArray();

        return new string(firstLetterOfEachWord);
    }

    private static IReadOnlyCollection<string> SplitIntoWords(string value)
    {
        var specialCharacters = value
            .Where(x => !char.IsLetterOrDigit(x))
            .ToArray();

        return !specialCharacters.Any()
            ? new[] { value }
            : value.Split(specialCharacters, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }
}