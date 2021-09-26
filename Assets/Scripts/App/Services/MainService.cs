using System;
using Dto;
using SocketServer.Decorator;
using UnityEngine;

namespace App.Services
{
    public class MainService : IHandler<MainServiceInDto, MainServiceOutDto>
    {
        private readonly ITaskScheduler _taskScheduler;

        public MainService(ref ITaskScheduler taskScheduler)
        {
            _taskScheduler = taskScheduler;
        }
        
        public MainServiceOutDto Handle(MainServiceInDto input)
        {
            _taskScheduler.ScheduleTask(new FrameDescriptionTask(input.text.TrimEnd('\0')));
            
            var output = new MainServiceOutDto {
                someContent = "Yes!"
            };
            return output;
        }

        public void OnSceneUpdated()
        {
            _taskScheduler.ScheduleTask(new TakeScreenshotTask());
        }
    }
}