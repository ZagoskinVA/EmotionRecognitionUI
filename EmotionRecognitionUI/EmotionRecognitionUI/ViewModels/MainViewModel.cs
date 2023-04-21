using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EmotionRecognitionUI.Events;
using EmotionRecognitionUI.Services;
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

        var trainingViewModel = new TrainingViewModel(eventAggregator, new YandexDriveService());
        CurrentContentPage = new HomeViewModel(eventAggregator, trainingViewModel);
        
        MenuPage = new MenuViewModel(eventAggregator, new ObservableCollection<MenuItemViewModel>()
        {
            new MenuItemViewModel("Главная", new HomeViewModel(eventAggregator, trainingViewModel), eventAggregator),
            new MenuItemViewModel("Тренажёр", trainingViewModel, eventAggregator)
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