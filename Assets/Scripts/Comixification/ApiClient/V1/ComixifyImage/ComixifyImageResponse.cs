using System.IO;

namespace Comixification.ApiClient.V1.ComixifyImage
{
    public class ComixifyImageResponse
    {
        public Stream ImageContent { get; }

        public ComixifyImageResponse(Stream imageContent)
        {
            ImageContent = imageContent;
        }
    }
}