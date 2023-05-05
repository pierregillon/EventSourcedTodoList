namespace TimeOnion.Shared.MVU;

public interface IStore
{
    T GetState<T>() where T : IState;
    void SetState<T>(T state) where T : IState;
}