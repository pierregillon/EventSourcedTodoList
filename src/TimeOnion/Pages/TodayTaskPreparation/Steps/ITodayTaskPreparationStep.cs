namespace TimeOnion.Pages.TodayTaskPreparation.Steps;

public interface ITodayTaskPreparationStep
{
    TodayTaskPreparationSteps Id { get; }
    ITodayTaskPreparationStep? Next();
    Task Save(TodayTaskPreparationState state);
    Task<TodayTaskPreparationState> Initialize(TodayTaskPreparationState state);
}