using System.Windows.Input;
using Avalonia.Media.Imaging;
using EmotionRecognitionUI.Events;
using Prism.Commands;
using Prism.Events;
using ReactiveUI;

namespace EmotionRecognitionUI.ViewModels;

public class AnswerViewModel: ViewModelBase
{
    public ICommand CloseAnswerCommand { get; }

    public string Result { get; set; }

    public Bitmap Image { get; set; }

    private readonly IEventAggregator _eventAggregator;
    private string _textBoxForeground = "White";
    public string TextBoxForeground
    {
        get => _textBoxForeground;
        set => this.RaiseAndSetIfChanged(ref _textBoxForeground, value);
    }

    public AnswerViewModel(IEventAggregator eventAggregator, bool isCorrect, string correctAnswer, Bitmap image)
    {
        _eventAggregator = eventAggregator;
        _eventAggregator.GetEvent<ChangeThemeModeEvent>().Subscribe((foreground) =>
        {
            TextBoxForeground = foreground;
        });
        
        CloseAnswerCommand = new DelegateCommand(OnCloseAnswer);
        var correct = correctAnswer == "страхудивление" ? "страх и удивление" : correctAnswer;
        Result = isCorrect ? "Ответ правильный" : $"Вы не угадали. Правильный ответ {correct}";
        Image = image;
    }

    private void OnCloseAnswer()
    {
        _eventAggregator.GetEvent<CloseAnswerPageEvent>().Publish();
    }
}