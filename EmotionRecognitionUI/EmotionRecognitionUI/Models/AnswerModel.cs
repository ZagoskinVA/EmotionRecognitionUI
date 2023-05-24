using Avalonia.Media.Imaging;

namespace EmotionRecognitionUI.Models;

public class AnswerModel
{
    public bool IsCorrect { get; set; }
    public string CorrectAnswer { get; set; }

    public Bitmap Image { get; set; }
}