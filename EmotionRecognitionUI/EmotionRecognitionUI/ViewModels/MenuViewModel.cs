using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using EmotionRecognitionUI.Events;
using Prism.Commands;
using Prism.Events;
using ReactiveUI;

namespace EmotionRecognitionUI.ViewModels;

public class MenuViewModel: ViewModelBase
{
    private IEventAggregator _eventAggregator;
    private object selectedItem;

    public object SelectedItem
    {
        get => selectedItem;
        set => this.RaiseAndSetIfChanged(ref selectedItem, value);
    }
    public ObservableCollection<MenuItemViewModel> MenuItems { get; }

    public ICommand SelectMenuItemCommand { get; }
    
    public MenuViewModel(IEventAggregator eventAggregator, ObservableCollection<MenuItemViewModel> menuItems)
    {
        _eventAggregator = eventAggregator;
        eventAggregator.GetEvent<MenuItemSelectedEvent>().Subscribe(OnSelectMenuItem);
        MenuItems = menuItems;
        SelectedItem = menuItems.First();
        SelectMenuItemCommand = new DelegateCommand<SelectionChangedEventArgs>(OnSelectMenuItem);
        
    }

    private void OnSelectMenuItem(SelectionChangedEventArgs args)
    {
        if (args != null && args.AddedItems?.Count > 0 && args.AddedItems[0] is MenuItemViewModel itemViewModel)
        {
            SelectedItem = MenuItems.First(x => x.Choice == itemViewModel.Choice);
            if (args.Source != null)
            {
                _eventAggregator.GetEvent<MenuMovingEvent>().Publish(itemViewModel.MenuItemContent);
            }
            
        }
    }
}