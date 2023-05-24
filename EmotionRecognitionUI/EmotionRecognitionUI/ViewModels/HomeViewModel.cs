using System.Windows.Input;
using Avalonia.Controls;
using EmotionRecognitionUI.Events;
using EmotionRecognitionUI.Services;
using Prism.Commands;
using Prism.Events;
using ReactiveUI;

namespace EmotionRecognitionUI.ViewModels;

public class HomeViewModel: ViewModelBase
{
    public string Greeting { get; set; } = "Тренажёр для определения эмоций!";
    private IEventAggregator _eventAggregator;
    private readonly TrainingViewModel _trainingViewModel;
    public ICommand StartTrainingCommand { get; }
    
    private string _textBoxForeground = "White";
    public string TextBoxForeground
    {
        get => _textBoxForeground;
        set => this.RaiseAndSetIfChanged(ref _textBoxForeground, value);
    }

    public HomeViewModel(IEventAggregator eventAggregator, TrainingViewModel trainingViewModel)
    {
        _trainingViewModel = trainingViewModel;
        _eventAggregator = eventAggregator;
        StartTrainingCommand = new DelegateCommand(OnStartTraining);
        eventAggregator.GetEvent<ChangeThemeModeEvent>().Subscribe((foreground) =>
        {
            TextBoxForeground = foreground;
        });
    }
    
    
    private void OnStartTraining()
    {
        _eventAggregator.GetEvent<MenuItemSelectedEvent>()
            .Publish(new SelectionChangedEventArgs(null, null,
                new[] {new MenuItemViewModel("Тренажёр",
                    _trainingViewModel, _eventAggregator) { }}));
    }
}