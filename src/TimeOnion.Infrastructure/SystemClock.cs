using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Infrastructure;

public class SystemClock : IClock
{
    public DateTime Now() => DateTime.UtcNow;
}