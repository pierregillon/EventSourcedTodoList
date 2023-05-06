using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodayTaskPreparation.Actions;

internal record MoveToNextPreparationStepAction : IAction<TodayTaskPreparationState>;

internal class MoveToNextPreparationStepActionHandler :
    ActionHandlerBase<TodayTaskPreparationState, MoveToNextPreparationStepAction>
{
    public MoveToNextPreparationStepActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodayTaskPreparationState> Apply(
        TodayTaskPreparationState state,
        MoveToNextPreparationStepAction action
    )
    {
        if (state.CurrentStep is not null)
        {
            await state.CurrentStep.Save(state);

            state = state with { CurrentStep = state.CurrentStep.Next() };

            if (state.CurrentStep is not null)
            {
                state = await state.CurrentStep.Initialize(state);
            }
        }

        return state;
    }
}