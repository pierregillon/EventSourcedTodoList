using Bunit;
using MudBlazor;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Shared.MVU;

namespace TimeOnion.WebApp.Tests.Unit;

public abstract class ComponentTestBase : BUnitTest
{
    protected ICommandDispatcher CommandDispatcher => GetService<ICommandDispatcher>();
    protected IQueryDispatcher QueryDispatcher => GetService<IQueryDispatcher>();

    protected T GetState<T>() where T : IState => GetService<IStore>().GetState<T>();

    // For an unknown reason, 'component.Find("button:not(disabled):contains('Mettre à jour')").Click()' does not work.
    protected async Task SubmitFormFromFirstButton(IRenderedFragment component) =>
        await Renderer.Dispatcher.InvokeAsync(async () =>
            await component.FindComponent<MudButton>().Instance.OnClick.InvokeAsync()
        );
}