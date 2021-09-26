using System.IO;
using TopRightMenu.Events;
using UnityEngine;

namespace Comixification.Command.ComixifyImage
{
    public class ComixifyImageInterceptor
    {
        public ComixifyImageInterceptor(ComixifyImageHandler handler, EventBus eventBus)
        {
            var command = new ComixifyImageCommand();
            
            eventBus.SubscribeComixifierChosenEvent(delegate(Comixifier.Comixifier comixifier)
            {
                command.Comixifier = comixifier;

                if (IsCommandReady(command))
                {
                    handler.Handle(command);
                    command.Image = null;
                }
            });

            eventBus.SubscribeSceneScreenshotReadyEvent(delegate(Texture2D screenshot)
            {
                command.Image = screenshot;

                if (IsCommandReady(command))
                {
                    handler.Handle(command);
                    command.Image = null;
                }
            });
        }

        private bool IsCommandReady(ComixifyImageCommand command)
        {
            if (command.Comixifier == null)
            {
                return false;
            }

            if (command.Image == null)
            {
                return false;
            }

            return true;
        }
    }
}
