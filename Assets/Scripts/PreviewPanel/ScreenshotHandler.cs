using System;
using TopRightMenu;
using TopRightMenu.Events;
using UnityEngine;
using Zenject;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;

    private Camera myCamera;
    private bool takeScreenshotOnNextFrame;

    public static volatile Texture2D LastScreenshot;
    public static Rect LastRect;
    private int ScreenshotNumber;

    private bool isCameraButtonHeld;

    public delegate void ScreenshotReadyHandler(Texture2D screenshot);
    public event ScreenshotReadyHandler OnScreenshotReady;

    private EventBus _eventBus;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.SubscribeComixifierInitializedEvent(RunTakingScreenshot);
    }
    
    private void Awake()
    {
        instance = this;
        myCamera = Camera.main;

        var toggleComixifier = FindObjectOfType<ToggleComixifier>();
        toggleComixifier.SubscribeHandlerRunImageComixifying(RunTakingScreenshot);
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            transform.position += 0.1f * new Vector3(0.1f *-Input.GetAxisRaw("Horizontal"), 0, 0.1f * -Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            transform.position += 2.0f * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !isCameraButtonHeld) {
            isCameraButtonHeld = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1) && isCameraButtonHeld) {
            isCameraButtonHeld = false;
        }
        if (isCameraButtonHeld) {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {
                var y = 3f * Input.GetAxis("Mouse X");
                var x = 3f * Input.GetAxis("Mouse Y");
                transform.eulerAngles -= new Vector3(x, y * -1, 0);
            }   
        }
    }

    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
    
            LastRect = rect;
    
            // SaveScreenShot(renderResult, renderTexture.width, renderTexture.height);
    
            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
            
            _eventBus.InvokeSceneScreenshotReadyEvent(renderResult);
            
            // OnScreenshotReady?.Invoke(renderResult);
        }
    }
    private void SaveScreenShot(Texture2D render, int width, int height)
    {
        byte[] byteArray = render.EncodeToPNG();
        string path = Application.streamingAssetsPath + string.Format(@"/{0}.png", "test_" +  DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")); //TODO: replace \ to /
        System.IO.File.WriteAllBytes(path, byteArray);

        LastScreenshot = new Texture2D(width, height);
        LastScreenshot.LoadImage(byteArray);
    }
    
    public void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 32);
        takeScreenshotOnNextFrame = true;
    }
    public static void TakeScreenshot_Static(int width, int height, int index)
    {
        instance.TakeScreenshot(width, height);
        instance.ScreenshotNumber = index;
    }

    public void SubscribeHandlerOnScreenshotReady(ScreenshotReadyHandler handler)
    {
        OnScreenshotReady += handler;
    }

    private void RunTakingScreenshot()
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(160 * 8, 90 * 8, 32);
        takeScreenshotOnNextFrame = true;
    }
}
