using System.Windows.Input;
using EmotionRecognitionUI.Events;
using Prism.Commands;
using Prism.Events;

namespace EmotionRecognitionUI.ViewModels;

public class AnswerViewModel: ViewModelBase
{
    public ICommand CloseAnswerCommand { get; }

    public string Result { get; set; }

    private readonly IEventAggregator _eventAggregator;

    public AnswerViewModel(IEventAggregator eventAggregator, bool isCorrect)
    {
        _eventAggregator = eventAggregator;
        CloseAnswerCommand = new DelegateCommand(OnCloseAnswer);
        Result = isCorrect ? "Ответ правильный" : "Вы не угадали";
    }

    private void OnCloseAnswer()
    {
        _eventAggregator.GetEvent<CloseAnswerPageEvent>().Publish();
    }
}