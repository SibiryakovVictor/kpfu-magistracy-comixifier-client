using System.IO;
using UnityEngine;

namespace Comixification.ApiClient.V1.ComixifyImage
{
    public class ComixifyImageRequest
    {
        public Stream Settings { get; }

        public Texture2D Image { get; }

        public ComixifyImageRequest(Stream settings, Texture2D image)
        {
            Settings = settings;
            Image = image;
        }
    }
}
