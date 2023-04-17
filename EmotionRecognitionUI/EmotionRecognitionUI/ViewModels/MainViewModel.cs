using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EmotionRecognitionUI.Events;
using Prism.Commands;
using Prism.Events;
using ReactiveUI;

namespace EmotionRecognitionUI.ViewModels;

public class MainViewModel : ViewModelBase
{
    #region private properties
    
    private bool isOpen = true;
    private object currentContentPage;
    private object menuPage;
    
    #endregion

    #region public properties

    public bool IsOpen { get => isOpen;
        set => this.RaiseAndSetIfChanged(ref isOpen, value);
    }

    public object CurrentContentPage
    {
        get => currentContentPage;
        set => this.RaiseAndSetIfChanged(ref currentContentPage, value);
    }
    
    public object MenuPage
    {
        get => menuPage;
        set => this.RaiseAndSetIfChanged(ref menuPage, value);
    }

    public ICommand ChangePaneCommand { get; }

    #endregion
    
    
    

    public MainViewModel(IEventAggregator eventAggregator)
    {
        eventAggregator.GetEvent<MenuMovingEvent>().Subscribe(OnMenuMoving);
        
        CurrentContentPage = new HomeViewModel(eventAggregator);
        
        MenuPage = new MenuViewModel(eventAggregator, new ObservableCollection<MenuItemViewModel>()
        {
            new MenuItemViewModel("Главная", new HomeViewModel(eventAggregator), eventAggregator),
            new MenuItemViewModel("Тренажёр", new TrainingViewModel(), eventAggregator)
        });
        
        ChangePaneCommand = new DelegateCommand(OnChangePane);
    }

    private void OnChangePane()
    {
        IsOpen = !IsOpen;
    }
    
    private void OnMenuMoving(ViewModelBase menuItemContent)
    {
        CurrentContentPage = menuItemContent;
    }
}