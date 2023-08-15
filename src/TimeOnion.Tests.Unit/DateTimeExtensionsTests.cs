using FluentAssertions;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Tests.Unit;

public class DateTimeExtensionsTests
{
    [Fact]
    public void In_first_quarter()
    {
        var inFirstQuarter = new[]
        {
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 2),
            new DateTime(2023, 2, 3),
            new DateTime(2023, 3, 20, 10, 0, 0)
        };

        inFirstQuarter
            .Select(x => x.GetQuarterBegin())
            .Distinct()
            .Single()
            .Should()
            .Be(new DateTime(2023, 1, 1));
    }

    [Fact]
    public void In_second_quarter()
    {
        var inFirstQuarter = new[]
        {
            new DateTime(2023, 4, 1),
            new DateTime(2023, 4, 2),
            new DateTime(2023, 5, 3),
            new DateTime(2023, 6, 20, 10, 0, 0)
        };

        inFirstQuarter
            .Select(x => x.GetQuarterBegin())
            .Distinct()
            .Single()
            .Should()
            .Be(new DateTime(2023, 4, 1));
    }

    [Fact]
    public void In_third_quarter()
    {
        var inFirstQuarter = new[]
        {
            new DateTime(2023, 7, 1),
            new DateTime(2023, 7, 2),
            new DateTime(2023, 8, 3),
            new DateTime(2023, 9, 20, 10, 0, 0)
        };

        inFirstQuarter
            .Select(x => x.GetQuarterBegin())
            .Distinct()
            .Single()
            .Should()
            .Be(new DateTime(2023, 7, 1));
    }

    [Fact]
    public void In_fourth_quarter()
    {
        var inFirstQuarter = new[]
        {
            new DateTime(2023, 10, 1),
            new DateTime(2023, 10, 2),
            new DateTime(2023, 11, 3),
            new DateTime(2023, 12, 20, 10, 0, 0)
        };

        inFirstQuarter
            .Select(x => x.GetQuarterBegin())
            .Distinct()
            .Single()
            .Should()
            .Be(new DateTime(2023, 10, 1));
    }

    [Fact]
    public void Get_week_begin()
    {
        new DateTime(2023, 1, 1)
            .GetWeekBegin()
            .Should()
            .Be(new DateTime(2022, 12, 26));

        new DateTime(2023, 1, 9)
            .GetWeekBegin()
            .Should()
            .Be(new DateTime(2023, 1, 9));

        new DateTime(2023, 3, 26)
            .GetWeekBegin()
            .Should()
            .Be(new DateTime(2023, 3, 20));
    }
}