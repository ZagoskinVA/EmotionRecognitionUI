using System;
using System.Windows.Input;
using Prism.Commands;
using ReactiveUI;

namespace EmotionRecognitionUI.ViewModels;

public class MainViewModel : ViewModelBase
{
    private bool isOpen = true;
    public bool IsOpen { get => isOpen;
        set => this.RaiseAndSetIfChanged(ref isOpen, value);
    }

    public object CurrentContentPage { get; set; } = new HomeViewModel();
    public object MenuPage { get; } = new MenuViewModel();

    public ICommand ChangePaneCommand { get; }

    public MainViewModel()
    {
        ChangePaneCommand = new DelegateCommand(OnChangePane);
    }

    private void OnChangePane()
    {
        IsOpen = !IsOpen;
    }
}