using Newtonsoft.Json;

namespace Comixification.ApiClient.V1.Progress
{
    public class ProgressRequest
    {
        [JsonProperty(PropertyName = "transformId")]
        public string TransformId { get; }

        public ProgressRequest(string transformId)
        {
            TransformId = transformId;
        }
    }
}