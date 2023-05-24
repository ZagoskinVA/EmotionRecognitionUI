using EmotionRecognitionUI.Events;
using Prism.Events;
using ReactiveUI;

namespace EmotionRecognitionUI.ViewModels;

public class DescriptionViewModel: ViewModelBase
{
    private string _textBoxForeground = "White";
    public string TextBoxForeground
    {
        get => _textBoxForeground;
        set => this.RaiseAndSetIfChanged(ref _textBoxForeground, value);
    }

    public DescriptionViewModel(IEventAggregator eventAggregator)
    {
        eventAggregator.GetEvent<ChangeThemeModeEvent>().Subscribe((foreground) =>
        {
            TextBoxForeground = foreground;
        });

    }
}