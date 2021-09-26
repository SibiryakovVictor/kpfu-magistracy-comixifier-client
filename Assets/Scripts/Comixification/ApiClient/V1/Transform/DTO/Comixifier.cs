using Newtonsoft.Json;

namespace Comixification.ApiClient.V1.Transform.DTO
{
    public class Comixifier
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; }
        
        [JsonProperty(PropertyName = "config")]
        public Config.Config Config { get; }

        public Comixifier(string name, Config.Config config)
        {
            Name = name;
            Config = config;
        }
    }
}
