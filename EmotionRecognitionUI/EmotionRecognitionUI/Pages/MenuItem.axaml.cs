using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EmotionRecognitionUI.Pages;

public partial class MenuItem : UserControl
{
    public MenuItem()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}