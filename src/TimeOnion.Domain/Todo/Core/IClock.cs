namespace TimeOnion.Domain.Todo.Core;

public interface IClock
{
    DateTime Now();
}