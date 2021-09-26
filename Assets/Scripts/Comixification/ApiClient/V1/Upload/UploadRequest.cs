using UnityEngine;

namespace Comixification.ApiClient.V1.Upload
{
    public class UploadRequest
    {
        public Texture2D Image { get; }
        
        public UploadRequest(Texture2D image)
        {
            Image = image;
        }
    }
}