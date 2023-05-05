namespace TimeOnion.Shared.MVU;

public interface IBlazorStateComponent
{
    void ReRender();
    string Id { get; }
}