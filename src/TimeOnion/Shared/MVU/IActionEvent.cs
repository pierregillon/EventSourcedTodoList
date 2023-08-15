namespace TimeOnion.Shared.MVU;

public interface IActionEvent<TState> : IActionEvent where TState : IState
{
}

public interface IActionEvent
{
}