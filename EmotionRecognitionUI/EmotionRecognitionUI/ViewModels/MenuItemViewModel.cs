using Prism.Events;

namespace EmotionRecognitionUI.ViewModels;

public class MenuItemViewModel : ViewModelBase
{
    public string Choice { get; }

    public ViewModelBase MenuItemContent { get; }

    public MenuItemViewModel(string choice, ViewModelBase menuItemContent, IEventAggregator eventAggregator)
    {
        Choice = choice;
        MenuItemContent = menuItemContent;
    }
}