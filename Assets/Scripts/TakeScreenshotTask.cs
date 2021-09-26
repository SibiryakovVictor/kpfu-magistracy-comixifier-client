using static UnityEngine.Object;

public class TakeScreenshotTask : ITask
{
    public void Execute()
    {
        var screenshotHandler = FindObjectOfType<ScreenshotHandler>();
        screenshotHandler.TakeScreenshot(160 * 6, 90 * 6);
    }
}