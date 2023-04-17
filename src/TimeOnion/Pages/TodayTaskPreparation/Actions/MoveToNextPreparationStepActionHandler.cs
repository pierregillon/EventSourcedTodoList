using BlazorState;

namespace TimeOnion.Pages.TodayTaskPreparation.Actions;

public class MoveToNextPreparationStepActionHandler : ActionHandler<TodayTaskPreparationState.MoveToNextPreparationStep>
{
    public MoveToNextPreparationStepActionHandler(IStore aStore) : base(aStore)
    {
    }

    public override async Task Handle(TodayTaskPreparationState.MoveToNextPreparationStep aAction,
        CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodayTaskPreparationState>();

        if (state.CurrentStep is not null)
        {
            await state.CurrentStep.Save(state);

            state.CurrentStep = state.CurrentStep.Next();

            if (state.CurrentStep is not null)
            {
                await state.CurrentStep.Initialize(state);
            }
        }
    }
}