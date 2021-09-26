using static UnityEngine.Object;

public class FrameDescriptionTask : ITask
{
    private readonly string _sentence;

    public FrameDescriptionTask(string sentence)
    {
        _sentence = sentence;
    }
    
    public void Execute()
    {
        var frameDesc = FindObjectOfType<FrameDescription>();
        frameDesc.MarkAndParseTree(_sentence);
    }
}