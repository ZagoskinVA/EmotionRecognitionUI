using EmotionRecognitionUI.Events;
using EmotionRecognitionUI.Models;
using EmotionRecognitionUI.Services;
using Prism.Events;
using ReactiveUI;

namespace EmotionRecognitionUI.ViewModels;

public class TrainingViewModel : ViewModelBase
{
    private readonly IYandexDriveService _yandexDriveService;
    private readonly IEventAggregator _eventAggregator;
    private object _currentPage;
    private object CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    public TrainingViewModel(IEventAggregator eventAggregator, IYandexDriveService yandexDriveService)
    {
        _yandexDriveService = yandexDriveService;
        _eventAggregator = eventAggregator;
        _currentPage = new GameViewModel(yandexDriveService, eventAggregator);
        eventAggregator.GetEvent<ShowResultEvent>().Subscribe(OnShowResult);
        eventAggregator.GetEvent<RefreshTrainingEvent>().Subscribe(OnRefreshTraining);
    }
    
    private void OnShowResult(ScoringModel scoringModel)
    {
        CurrentPage = new ResultViewModel(scoringModel, _eventAggregator);
    }

    private void OnRefreshTraining()
    {
        CurrentPage = new TrainingViewModel(_eventAggregator, _yandexDriveService);
    }

}