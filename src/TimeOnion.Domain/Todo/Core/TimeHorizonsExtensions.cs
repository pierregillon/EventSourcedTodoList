namespace TimeOnion.Domain.Todo.Core;

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

    public static TimeSpan ToTimeSpan(this TimeHorizons timeHorizon)
    {
        var reference = new DateTime();

        return timeHorizon switch
        {
            TimeHorizons.ThisLife => TimeSpan.MaxValue,
            TimeHorizons.ThisYear => reference.AddYears(1).Subtract(reference),
            TimeHorizons.ThisQuarter => reference.AddMonths(3).Subtract(reference),
            TimeHorizons.ThisMonth => reference.AddMonths(1).Subtract(reference),
            TimeHorizons.ThisWeek => TimeSpan.FromDays(7),
            TimeHorizons.ThisDay => TimeSpan.FromDays(1),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

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