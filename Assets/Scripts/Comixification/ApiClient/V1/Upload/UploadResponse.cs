using Newtonsoft.Json;

namespace Comixification.ApiClient.V1.Upload
{
    public class UploadResponse
    {
        [JsonProperty(PropertyName = "uid")]
        public string Uid { get; }

        public UploadResponse(string uid)
        {
            Uid = uid;
        }
    }
}