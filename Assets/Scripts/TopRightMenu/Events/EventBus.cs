using System.IO;
using UnityEngine;

namespace TopRightMenu.Events
{
    public class EventBus
    {
        public delegate void ComixifierInitializedHandler();
        private event ComixifierInitializedHandler ComixifierInitializedEvent;

        public delegate void SceneScreenshotReadyHandler(Texture2D screenshot);
        private event SceneScreenshotReadyHandler SceneScreenshotReadyEvent;

        public delegate void ComixifierParametersPreparedHandler(Stream parameters);
        private event ComixifierParametersPreparedHandler ComixifierParametersPreparedEvent;

        public delegate void GotComixifiedImageHandler(string imgPath);
        private event GotComixifiedImageHandler GotComixifiedImageEvent;

        public delegate void ComixifierChosenHandler(Comixification.Comixifier.Comixifier comixifier);
        private event ComixifierChosenHandler ComixifierChosenEvent;
        
        public void SubscribeComixifierInitializedEvent(ComixifierInitializedHandler handler)
        {
            ComixifierInitializedEvent += handler;
        }

        public void InvokeComixifierInitializedEvent()
        {
            ComixifierInitializedEvent?.Invoke();
        }

        public void SubscribeSceneScreenshotReadyEvent(SceneScreenshotReadyHandler handler)
        {
            SceneScreenshotReadyEvent += handler;
        }

        public void InvokeSceneScreenshotReadyEvent(Texture2D screenshot)
        {
            SceneScreenshotReadyEvent?.Invoke(screenshot);
        }

        public void SubscribeComixifierParametersPreparedEvent(ComixifierParametersPreparedHandler handler)
        {
            ComixifierParametersPreparedEvent += handler;
        }

        public void InvokeComixifierParametersPreparedEvent(Stream parameters)
        {
            ComixifierParametersPreparedEvent?.Invoke(parameters);
        }
        
        public void SubscribeGotComixifiedImageEvent(GotComixifiedImageHandler handler)
        {
            GotComixifiedImageEvent += handler;
        }

        public void InvokeGotComixifiedImageEvent(string imgPath)
        {
            GotComixifiedImageEvent?.Invoke(imgPath);
        }

        public void SubscribeComixifierChosenEvent(ComixifierChosenHandler handler)
        {
            ComixifierChosenEvent += handler;
        }

        public void InvokeComixifierChosenEvent(Comixification.Comixifier.Comixifier comixifier)
        {
            ComixifierChosenEvent?.Invoke(comixifier);
        }
    }
}
