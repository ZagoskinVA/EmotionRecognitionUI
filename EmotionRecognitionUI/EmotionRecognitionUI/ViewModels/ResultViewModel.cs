using System.Windows.Input;
using EmotionRecognitionUI.Events;
using EmotionRecognitionUI.Models;
using Prism.Commands;
using Prism.Events;
using ReactiveUI;

namespace EmotionRecognitionUI.ViewModels;

public class ResultViewModel: ViewModelBase
{
    private readonly IEventAggregator _eventAggregator;

    public string Score { get; set; }

    public string Accuracy { get; set; }

    public ICommand RefreshCommand { get; }


    public ResultViewModel(ScoringModel scoreModel, IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        Score = $"Правильных ответов {scoreModel.CorrectAnswers} из {scoreModel.AnswersCount}";
        Accuracy = $"Ваша точность {(scoreModel.Accuracy * 100):N2} %";
        RefreshCommand = new DelegateCommand(OnRefresh);
    }

    private void OnRefresh()
    {
        _eventAggregator.GetEvent<RefreshTrainingEvent>().Publish();
    }
}