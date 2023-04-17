namespace TimeOnion.Pages.TodayTaskPreparation.Steps;

public interface ITodayTaskPreparationStep
{
    TodayTaskPreparationSteps Id { get; }
    ITodayTaskPreparationStep? Next();
    Task Save(TodayTaskPreparationState state);
    Task Initialize(TodayTaskPreparationState state);
}