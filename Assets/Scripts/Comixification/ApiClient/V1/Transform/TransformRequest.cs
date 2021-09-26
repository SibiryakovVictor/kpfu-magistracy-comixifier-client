using Newtonsoft.Json;
using UnityEngine;

namespace Comixification.ApiClient.V1.Transform
{
    public class TransformRequest
    {
        [JsonProperty(PropertyName = "comixifier")]
        public DTO.Comixifier Comixifier { get; }

        public Texture2D Image { get; }

        public TransformRequest(Texture2D image, DTO.Comixifier comixifier)
        {
            Image = image;
            Comixifier = comixifier;
        }
    }
}