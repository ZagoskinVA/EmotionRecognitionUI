using EmotionRecognitionUI.Models;
using Prism.Events;

namespace EmotionRecognitionUI.Events;

public class ShowResultEvent: PubSubEvent<ScoringModel>
{
    
}