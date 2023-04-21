using Avalonia.Media.Imaging;

namespace EmotionRecognitionUI.Models;

public class ImageModel
{
    public Bitmap Image { get; set; }
    public string Label { get; set; }
}