using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EmotionRecognitionUI.Pages;

public partial class ResultPage : UserControl
{
    public ResultPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}