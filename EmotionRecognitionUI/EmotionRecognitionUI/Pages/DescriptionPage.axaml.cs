using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EmotionRecognitionUI.Pages;

public partial class DescriptionPage : UserControl
{
    public DescriptionPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}