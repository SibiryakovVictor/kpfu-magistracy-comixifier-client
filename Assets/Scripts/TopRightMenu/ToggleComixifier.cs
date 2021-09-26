using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

namespace TopRightMenu
{
    public class ToggleComixifier : MonoBehaviour
    {
        public GameObject buttonTooltip;
        
        [SerializeField]
        private Toggle toggle;

        [SerializeField]
        private Button runButton;
        
        [SerializeField]
        private Dropdown comixifiers;

        private string _lastImgPath;
        
        public delegate void RunImageComixifyingHandler();
        private event RunImageComixifyingHandler OnRunImageComixifying;

        public delegate void GotProcessedImageHandler(string imgPath);
        private event GotProcessedImageHandler OnGotProcessedImage;

        void Awake()
        {
            buttonTooltip = gameObject.transform.FindDeepChild("Button Tooltip").gameObject;
            
            toggle = gameObject.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate
            {
                var panel = gameObject.transform.FindDeepChild("ComixifierMenu");
                panel.gameObject.SetActive(!toggle.isOn);
                
                buttonTooltip.SetActive(false);
            });
        }

        private async void ComixifyImage(Texture2D img)
        {
            var client = new HttpClient();

            Debug.Log("got screenshot");
            
            var imgStream = new MemoryStream(img.EncodeToPNG());
            var streamContent = new StreamContent(imgStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            var req = new HttpRequestMessage();
            req.RequestUri = new Uri("http://127.0.0.1:9001/");
            req.Content = streamContent;
            var resp = await client.SendAsync(req);

            var result = await resp.Content.ReadAsStreamAsync();
            
            var imgFileName = string.Format("{0}_{1}",
                ((int) DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds).ToString(),
                Guid.NewGuid().ToString().Substring(0, 8)
            );
            var imgFilePath = Directory.GetCurrentDirectory() + $@"/Images/{imgFileName}.png";
            
            var imgFile = File.Create(imgFilePath);
            await result.CopyToAsync(imgFile);
            result.Close();
            imgFile.Close();
            
            Debug.Log("done");
            
            OnGotProcessedImage?.Invoke(imgFilePath);
        }

        public void SubscribeHandlerOnGotProcessedImage(GotProcessedImageHandler handler)
        {
            OnGotProcessedImage += handler;
        }

        public void SubscribeHandlerRunImageComixifying(RunImageComixifyingHandler handler)
        {
            OnRunImageComixifying += handler;
        }
    }
}