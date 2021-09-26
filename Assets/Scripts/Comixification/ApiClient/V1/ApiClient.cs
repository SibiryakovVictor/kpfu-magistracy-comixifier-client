using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Comixification.ApiClient.V1.ComixifyImage;
using Comixification.ApiClient.V1.Download;
using Comixification.ApiClient.V1.Progress;
using Comixification.ApiClient.V1.Transform;
using Newtonsoft.Json;
using UnityEngine;

namespace Comixification.ApiClient.V1
{
    public class ApiClient
    {
        // public async Task<UploadResponse> Upload(UploadRequest req)
        // {
        //     var httpReq = new HttpRequestMessage();
        //     
        //     httpReq.RequestUri = new Uri("http://127.0.0.1:9001/upload");
        //     
        //     var imgStream = new MemoryStream(req.Image.EncodeToPNG());
        //     var streamContent = new StreamContent(imgStream);
        //     streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        //     httpReq.Content = streamContent;
        //     
        //     var client = new HttpClient();
        //     var resp = await client.SendAsync(httpReq);
        //
        //     var respContent = await resp.Content.ReadAsStringAsync();
        //
        //     return JsonConvert.DeserializeObject<UploadResponse>(respContent);
        // }
        
        public async Task<TransformResponse> Transform(TransformRequest req)
        {
            var httpReq = new HttpRequestMessage();
            
            httpReq.RequestUri = new Uri("http://127.0.0.1:9001/transform");
            
            httpReq.Headers.Add("Comixifier-Name", req.Comixifier.Name);
            httpReq.Headers.Add("Comixifier-Settings", JsonConvert.SerializeObject(req.Comixifier.Config));
            
            var imgStream = new MemoryStream(req.Image.EncodeToPNG());
            var streamContent = new StreamContent(imgStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            httpReq.Content = streamContent;
            
            var client = new HttpClient();
            var resp = await client.SendAsync(httpReq);

            var respContent = await resp.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<TransformResponse>(respContent);
        }
        
        public async Task<ProgressResponse> Progress(ProgressRequest req)
        {
            var httpReq = new HttpRequestMessage();
            
            httpReq.RequestUri = new Uri("http://127.0.0.1:9001/progress");
            httpReq.Content = new StringContent(JsonConvert.SerializeObject(req));
            
            var client = new HttpClient();
            var resp = await client.SendAsync(httpReq);

            var respContent = await resp.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<ProgressResponse>(respContent);
        }
        
        public async Task<DownloadResponse> Download(DownloadRequest req)
        {
            var httpReq = new HttpRequestMessage();
            
            httpReq.RequestUri = new Uri("http://127.0.0.1:9001/download");
            httpReq.Content = new StringContent(JsonConvert.SerializeObject(req));
            
            var client = new HttpClient();
            var resp = await client.SendAsync(httpReq);

            var respContent = await resp.Content.ReadAsStreamAsync();
            return new DownloadResponse(respContent);
        }
        
        public async Task<ComixifyImageResponse> ComixifyImage(ComixifyImageRequest req)
        {
            // var content = new StringContent("{\"azaza\": \"bzaza\"}", Encoding.UTF8, "application/json");
            // var response = await client.PostAsync("https://httpbin.org/post", content); // replace URL

            var httpReq = new HttpRequestMessage();
            
            httpReq.RequestUri = new Uri("http://127.0.0.1:9001/");
            
            httpReq.Headers.Add("Comixifier-Settings", new StreamReader(req.Settings).ReadToEnd());
            
            var imgStream = new MemoryStream(req.Image.EncodeToPNG());
            var streamContent = new StreamContent(imgStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            httpReq.Content = streamContent;
            
            var client = new HttpClient();
            var resp = await client.SendAsync(httpReq);

            // var resp = await client.GetAsync("https://picsum.photos/1600/900");
            var result = await resp.Content.ReadAsStreamAsync();

            return new ComixifyImageResponse(result);
        }

        public async Task<ComixifyImageResponse> Comixify(
            Comixifier.Comixifier comixifier, 
            Texture2D image, 
            Transform.DTO.Comixifier comixifierDto
        ) {
            var httpReq = new HttpRequestMessage();
            
            httpReq.RequestUri = new Uri("http://127.0.0.1:9001/");
            
            httpReq.Headers.Add("Comixifier-Name", comixifier.GetName());
            httpReq.Headers.Add("Comixifier-Settings", JsonConvert.SerializeObject(comixifierDto));
            
            var imgStream = new MemoryStream(image.EncodeToPNG());
            var streamContent = new StreamContent(imgStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            httpReq.Content = streamContent;
            
            var client = new HttpClient();
            var resp = await client.SendAsync(httpReq);
            
            var result = await resp.Content.ReadAsStreamAsync();
            
            return new ComixifyImageResponse(result);
        }
    }
}
