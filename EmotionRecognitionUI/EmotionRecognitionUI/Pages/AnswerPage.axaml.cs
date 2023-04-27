using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EmotionRecognitionUI.Pages;

public partial class AnswerPage : UserControl
{
    public AnswerPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}