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

    public static DateTime GetEndDate(this TimeHorizons timeHorizon, DateTime date) => 
        date
            .GetStartDate(timeHorizon)
            .Add(timeHorizon);

    private static DateTime GetStartDate(this DateTime date, TimeHorizons timeHorizon) => timeHorizon switch
    {
        TimeHorizons.ThisLife => default,
        TimeHorizons.ThisYear => new DateTime(date.Year, 1, 1),
        TimeHorizons.ThisQuarter => date.GetQuarterBegin(),
        TimeHorizons.ThisMonth => new DateTime(date.Year, date.Month, 1),
        TimeHorizons.ThisWeek => date.GetWeekBegin(),
        TimeHorizons.ThisDay => date.Date,
        _ => throw new ArgumentOutOfRangeException()
    };

    private static DateTime Add(this DateTime date, TimeHorizons timeHorizon) => timeHorizon switch
    {
        TimeHorizons.ThisLife => DateTime.MaxValue,
        TimeHorizons.ThisYear => date.AddYears(1),
        TimeHorizons.ThisQuarter => date.AddMonths(3),
        TimeHorizons.ThisMonth => date.AddMonths(1),
        TimeHorizons.ThisWeek => date.AddDays(7),
        TimeHorizons.ThisDay => date.AddDays(1),
        _ => throw new ArgumentOutOfRangeException()
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