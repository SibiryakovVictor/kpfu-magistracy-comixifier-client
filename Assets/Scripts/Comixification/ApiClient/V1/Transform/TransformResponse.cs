using Newtonsoft.Json;

namespace Comixification.ApiClient.V1.Transform
{
    public class TransformResponse
    {
        [JsonProperty(PropertyName = "transformId")]
        public string TransformId { get; }

        public TransformResponse(string transformId)
        {
            TransformId = transformId;
        }
    }
}