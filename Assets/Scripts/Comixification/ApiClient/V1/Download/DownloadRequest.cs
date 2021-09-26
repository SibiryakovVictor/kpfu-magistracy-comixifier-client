using Newtonsoft.Json;

namespace Comixification.ApiClient.V1.Download
{
    public class DownloadRequest
    {
        [JsonProperty(PropertyName = "transformId")]
        public string TransformId { get; }

        public DownloadRequest(string transformId)
        {
            TransformId = transformId;
        }
    }
}