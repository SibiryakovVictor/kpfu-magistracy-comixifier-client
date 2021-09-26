using System;
using App.Services;
using Dto;
using SocketServer;
using Unity.Jobs;
using UnityEngine;
using static UnityEngine.Object;

public struct MainJob : IJob
{
    public void Execute()
    {
        try {
            Debug.Log("Application started.");
            
            ITaskScheduler taskExecutor = FindObjectOfType<UnityMainThreadTaskExecutor>();
            var mainService = new MainService(ref taskExecutor);

            var messageConverter =
                new JsonMessageConverter<MainServiceInDto, MainServiceOutDto>(mainService, new MainServiceOutDto());

            var server = new Server(messageConverter);
            
            Debug.Log("Initialization is completed. Now run...");

            server.Run();
        } catch (Exception exception) {
            Debug.LogError(exception.Message);
            Application.Quit(1);
        }
    }
}