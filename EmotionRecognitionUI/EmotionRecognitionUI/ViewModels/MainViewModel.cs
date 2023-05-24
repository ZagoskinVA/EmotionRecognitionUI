using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Themes.Fluent;
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

    public ICommand ChangeModeCommand { get; }

    #endregion
    
    private string _textBoxForeground = "White";
    public string TextBoxForeground
    {
        get => _textBoxForeground;
        set => this.RaiseAndSetIfChanged(ref _textBoxForeground, value);
    }

    private IEventAggregator _eventAggregator;
    

    public MainViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        eventAggregator.GetEvent<MenuMovingEvent>().Subscribe(OnMenuMoving);
        eventAggregator.GetEvent<ChangeThemeModeEvent>().Subscribe((foreground) =>
        {
            TextBoxForeground = foreground;
        });

        var trainingViewModel = new TrainingViewModel(eventAggregator, new YandexDriveService());
        CurrentContentPage = new HomeViewModel(eventAggregator, trainingViewModel);
        
        MenuPage = new MenuViewModel(eventAggregator, new ObservableCollection<MenuItemViewModel>()
        {
            new MenuItemViewModel("Главная", new HomeViewModel(eventAggregator, trainingViewModel), eventAggregator),
            new MenuItemViewModel("Маркеры эмоций", new DescriptionViewModel(eventAggregator), eventAggregator),
            new MenuItemViewModel("Тренажёр", trainingViewModel, eventAggregator)
        });
        
        ChangePaneCommand = new DelegateCommand(OnChangePane);
        ChangeModeCommand = new DelegateCommand(OnChangeMode);
    }

    private void OnChangePane()
    {
        IsOpen = !IsOpen;
        FluentThemeHelper.Mode = FluentThemeMode.Light;
    }
    
    private void OnMenuMoving(ViewModelBase menuItemContent)
    {
        CurrentContentPage = menuItemContent;
    }

    private void OnChangeMode()
    {
        if (App.Current.Styles[0] is FluentTheme themeMode)
        {
            themeMode.Mode =  themeMode.Mode == FluentThemeMode.Dark ? FluentThemeMode.Light : FluentThemeMode.Dark;
            _eventAggregator.GetEvent<ChangeThemeModeEvent>().Publish(themeMode.Mode == FluentThemeMode.Dark ? "White": "Black");
        }
    }
}