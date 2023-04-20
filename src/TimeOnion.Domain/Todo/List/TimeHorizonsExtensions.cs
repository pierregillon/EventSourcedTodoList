namespace TimeOnion.Domain.Todo.List;

public static class TimeHorizonsExtensions
{
    public static string ToText(this TimeHorizons timeHorizons) => timeHorizons switch
    {
        TimeHorizons.ThisDay => "Ce jour",
        TimeHorizons.ThisWeek => "Cette semaine",
        TimeHorizons.ThisMonth => "Ce mois",
        TimeHorizons.ThisQuarter => "Ce trimestre",
        TimeHorizons.ThisYear => "Cette année",
        TimeHorizons.ThisLife => "Cette vie",
        _ => throw new ArgumentOutOfRangeException(nameof(timeHorizons), timeHorizons, null)
    };

    public static TimeHorizons Next(this TimeHorizons timeHorizons)
    {
        var next = timeHorizons + 1;
        var temporalities = Enum.GetValues<TimeHorizons>();
        return temporalities.Contains(next) ? next : temporalities.Last();
    }

    public static TimeHorizons Previous(this TimeHorizons timeHorizons)
    {
        var next = timeHorizons - 1;
        var temporalities = Enum.GetValues<TimeHorizons>();
        return temporalities.Contains(next) ? next : temporalities.First();
    }
}