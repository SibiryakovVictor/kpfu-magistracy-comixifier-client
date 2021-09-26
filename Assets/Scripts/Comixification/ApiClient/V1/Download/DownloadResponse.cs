using System.IO;
using UnityEngine;

namespace Comixification.ApiClient.V1.Download
{
    public class DownloadResponse
    {
        public Stream Image { get; }

        public DownloadResponse(Stream image)
        {
            Image = image;
        }
    }
}