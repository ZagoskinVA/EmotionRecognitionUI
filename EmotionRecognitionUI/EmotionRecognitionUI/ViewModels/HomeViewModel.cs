using System.Windows.Input;
using Avalonia.Controls;
using EmotionRecognitionUI.Events;
using Prism.Commands;
using Prism.Events;

namespace EmotionRecognitionUI.ViewModels;

public class HomeViewModel: ViewModelBase
{
    public string Greeting { get; set; } = "Тренажёр для определения эмоций!";
    private IEventAggregator _eventAggregator;
    public ICommand StartTrainingCommand { get; }

    public HomeViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        StartTrainingCommand = new DelegateCommand(OnStartTraining);
    }
    
    
    private void OnStartTraining()
    {
        _eventAggregator.GetEvent<MenuItemSelectedEvent>()
            .Publish(new SelectionChangedEventArgs(null, null,
                new[] {new MenuItemViewModel("Тренажёр", 
                    new TrainingViewModel(), _eventAggregator) { }}));
    }
}