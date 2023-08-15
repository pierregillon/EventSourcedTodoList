namespace TimeOnion.Domain.Todo.Core;

public static class DateTimeExtensions
{
    private const int MonthCountInQuarter = 3;
    private const int DayCountInWeek = 7;

    public static DateTime GetQuarterBegin(this DateTime date) =>
        new(date.Year, (date.Month - 1) / MonthCountInQuarter * MonthCountInQuarter + 1, 1);

    public static DateTime GetWeekBegin(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
    {
        var diff = (DayCountInWeek + (date.DayOfWeek - startOfWeek)) % DayCountInWeek;
        return date.AddDays(-1 * diff).Date;
    }
}