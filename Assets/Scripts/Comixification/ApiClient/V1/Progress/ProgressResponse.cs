using Newtonsoft.Json;

namespace Comixification.ApiClient.V1.Progress
{
    public class ProgressResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; }
        
        [JsonProperty(PropertyName = "error")]
        public string Error{ get; }

        public ProgressResponse(string status, string error)
        {
            Status = status;
            Error = error;
        }
    }
}