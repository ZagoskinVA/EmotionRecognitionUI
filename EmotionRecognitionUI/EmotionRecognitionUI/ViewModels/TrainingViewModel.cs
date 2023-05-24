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

    private object GamePage { get; set; }

    public TrainingViewModel(IEventAggregator eventAggregator, IYandexDriveService yandexDriveService)
    {
        _yandexDriveService = yandexDriveService;
        _eventAggregator = eventAggregator;
        _currentPage = new GameViewModel(yandexDriveService, eventAggregator);
        eventAggregator.GetEvent<ShowResultEvent>().Subscribe(OnShowResult);
        eventAggregator.GetEvent<RefreshTrainingEvent>().Subscribe(OnRefreshTraining);
        eventAggregator.GetEvent<CloseAnswerPageEvent>().Subscribe(OnCloseAnswerPage);
        eventAggregator.GetEvent<OpenAnswerPageEvent>().Subscribe(OnOpenAnswerPage);
    }
    
    private void OnShowResult(ScoringModel scoringModel)
    {
        CurrentPage = new ResultViewModel(scoringModel, _eventAggregator);
    }

    private void OnOpenAnswerPage(AnswerModel answer)
    {
        GamePage = CurrentPage;
        CurrentPage = new AnswerViewModel(_eventAggregator, answer.IsCorrect, answer.CorrectAnswer, answer.Image);
    }

    private void OnRefreshTraining()
    {
        _eventAggregator.GetEvent<ShowResultEvent>().Unsubscribe(OnShowResult);
        _eventAggregator.GetEvent<RefreshTrainingEvent>().Unsubscribe(OnRefreshTraining);
        _eventAggregator.GetEvent<CloseAnswerPageEvent>().Unsubscribe(OnCloseAnswerPage);
        _eventAggregator.GetEvent<OpenAnswerPageEvent>().Unsubscribe(OnOpenAnswerPage);
        CurrentPage = new TrainingViewModel(_eventAggregator, _yandexDriveService);
    }
    

    private void OnCloseAnswerPage()
    {
        CurrentPage = GamePage;
        if (CurrentPage is GameViewModel gameViewModel)
        {
            gameViewModel.MoveToNextImage();
        }
    }
}