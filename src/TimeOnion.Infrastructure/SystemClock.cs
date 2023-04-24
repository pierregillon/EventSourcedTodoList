using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Infrastructure;

public class SystemClock : IClock
{
    public DateTime Now() => DateTime.UtcNow;
}