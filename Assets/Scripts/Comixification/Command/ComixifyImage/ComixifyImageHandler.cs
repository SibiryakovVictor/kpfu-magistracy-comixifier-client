using System;
using System.IO;
using System.Threading.Tasks;
using Comixification.ApiClient.V1.Download;
using Comixification.ApiClient.V1.Progress;
using Comixification.ApiClient.V1.Transform;
using Comixification.ApiClient.V1.Transform.DTO.Config;
using Comixification.ApiClient.V1.Upload;
using Comixification.Comixifier;
using TopRightMenu.Events;
using UnityEngine;

namespace Comixification.Command.ComixifyImage
{
    public class ComixifyImageHandler
    {
        private readonly ApiClient.V1.ApiClient _api;

        private readonly EventBus _eventBus;
        
        public ComixifyImageHandler(ApiClient.V1.ApiClient api, EventBus eventBus)
        {
            _api = api;
            _eventBus = eventBus;
        }
        
        public async Task Handle(ComixifyImageCommand command)
        {
            var transformReq = new TransformRequest(
                command.Image, 
                new ApiClient.V1.Transform.DTO.Comixifier(command.Comixifier.GetName(), ToConfig(command.Comixifier))
            );
            Debug.Log("send transform");
            var transformResp = await _api.Transform(transformReq);
            Debug.Log("transformId: " + transformResp.TransformId);
            if (transformResp.TransformId == "")
            {
                return;
            }
            
            var status = "";
            var error = "";
            for (int i = 0; i < 10; i++)
            {
                if (status != "")
                {
                    await Task.Delay(10 * 1000);
                }
                
                if (status is "" or "WAIT")
                {
                    var progressReq = new ProgressRequest(transformResp.TransformId);
                    Debug.Log("send progress");
                    var progressResp = await _api.Progress(progressReq);
                    Debug.Log("progress: " + progressResp.Status);
                    status = progressResp.Status;
                    error = progressResp.Error;

                    if (status != "WAIT")
                    {
                        break;
                    }
                }
            }
            
            switch (status)
            {
                case "" or "WAIT":
                    Debug.Log("unexpected: status is empty or 'WAIT'");
                    return;
                case "FATAL":
                    Debug.Log("unexpected: status is 'FATAL', error: " + error);
                    return;
            }
            
            Debug.Log("send download");
            var downloadReq = new DownloadRequest(transformResp.TransformId);
            var downloadResp = await _api.Download(downloadReq);
            
            var imgFileName = string.Format("{0}_{1}",
                ((int) DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds).ToString(),
                Guid.NewGuid().ToString().Substring(0, 8)
            );
            var imgFileType = GetImageFileType(command.Comixifier);
            var imgFilePath = Directory.GetCurrentDirectory() + $@"/Images/{imgFileName}.{imgFileType}";
            var imgFile = File.Create(imgFilePath);
            
            downloadResp.Image.Position = 0;
            await downloadResp.Image.CopyToAsync(imgFile);
            imgFile.Close();
            downloadResp.Image.Close();
            
            _eventBus.InvokeGotComixifiedImageEvent(imgFilePath);
            
            // var resp = await _api.Comixify(
            //     command.Comixifier,
            //     command.Image,
            //     new ApiClient.V1.Transform.DTO.Comixifier(command.Comixifier.GetName(), ToConfig(command.Comixifier))
            // );
            //
            // var imgFileName = string.Format("{0}_{1}",
            //     ((int) DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds).ToString(),
            //     Guid.NewGuid().ToString().Substring(0, 8)
            // );
            // var imgFileType = GetImageFileType(command.Comixifier);
            // var imgFilePath = Directory.GetCurrentDirectory() + $@"/Images/{imgFileName}.{imgFileType}";
            // var imgFile = File.Create(imgFilePath);
            //
            // resp.ImageContent.Position = 0;
            // await resp.ImageContent.CopyToAsync(imgFile);
            // imgFile.Close();
            // resp.ImageContent.Close();
            //
            // _eventBus.InvokeGotComixifiedImageEvent(imgFilePath);
        }

        private Config ToConfig(Comixifier.Comixifier comixifier)
        {
            switch (comixifier)
            {
                case VanceAI:
                    dynamic vanceAI = comixifier;
                    return new VanceAIConfig(vanceAI);
                case Face2Comics:
                    dynamic face2Comics = comixifier;
                    return new Face2ComicsConfig(face2Comics);
                case CutOut:
                    dynamic cutOut = comixifier;
                    return new CutOutConfig(cutOut);
                default:
                    throw new Exception("unknown type of comixifier");
            }
        }

        private string GetImageFileType(Comixifier.Comixifier comixifier)
        {
            switch (comixifier)
            {
                case VanceAI:
                case CutOut:
                    return "png";
                case Face2Comics:
                    return "jpg";
                default:
                    throw new Exception("unknown type of comixifier");
            }
        }
    }
}
