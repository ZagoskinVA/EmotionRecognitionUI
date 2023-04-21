namespace EmotionRecognitionUI.Models;

public class ScoringModel
{
    public int CorrectAnswers { get; set; }
    public int AnswersCount { get; set; } = 1;
    
    public double Accuracy => (double)CorrectAnswers / AnswersCount ;
}