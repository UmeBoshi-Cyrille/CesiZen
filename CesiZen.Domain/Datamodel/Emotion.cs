using CesiZen.Domain.Enum;

namespace CesiZen.Domain.Datamodel;

public class Emotion : ADate
{
    public PrimaryEmotion PrimaryEmotion { get; set; }

    public SecondaryEmotion SecondaryEmotion { get; set; }
}
