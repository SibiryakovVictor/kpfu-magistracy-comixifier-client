using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadTaskExecutor : MonoBehaviour, ITaskScheduler
{
    private Queue<ITask> _taskQueue = new Queue<ITask>();
    private readonly object _queueLock = new object();

    private void Start()
    {
        Debug.Log($"[{GetType().Name}] started...");  
    }

    private void Update()
    {
        lock (_queueLock)
        {
            if (_taskQueue.Count > 0) {
                try {
                    _taskQueue.Dequeue().Execute();
                } catch (Exception exception) {
                    Debug.Log(exception.Message);    
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            Debug.Log("Left Alt!");
            var screenshotHandler = FindObjectOfType<ScreenshotHandler>();
            screenshotHandler.TakeScreenshot(160 * 12, 90 * 12);        
        }
    }
    
    public void ScheduleTask(ITask newTask)
    {
        lock (_queueLock)
        {
            _taskQueue.Enqueue(newTask);
        }
    }
}