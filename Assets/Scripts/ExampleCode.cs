using System.Net.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using Unity.Jobs;

public class ExampleCode : MonoBehaviour
{
    public event EventHandler<MessageAcceptedEventArgs> MessageAccepted;
    
    private Queue<ITask> _taskQueue = new Queue<ITask>();
    private readonly object _queueLock = new object();

    // static void MessageAcceptedHandler(object sender, MessageAcceptedEventArgs args)
    // {
    //     Debug.Log($"from event handler: {args.Message}");
    //
    //     try {
    //         var frameDescJob = new MainJob {Sentence = args.Message.TrimEnd('\0')};
    //         var handle = frameDescJob.Schedule();
    //
    //         handle.Complete();
    //     } catch (Exception exception) {
    //         Debug.Log(exception.Message);
    //     }
    // }
    
    // called zero
    void Awake()
    {
        Debug.Log("Awake");
    }

    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }

    // called third
    void Start()
    {
        Debug.Log("Start");

        // MessageAccepted += MessageAcceptedHandler;

        TcpListener listener = TcpListener.Create(8110);
        var storage = new Dictionary<string, object>
        {
            {"listener", listener}
        };

        listener.Start();
        Debug.Log("Started sync socket server on 8110...");

        listener.BeginAcceptSocket(HandleSocketConnection, storage);

        // Socket clientSocket = listener.AcceptSocket();
        // Debug.Log("Client is accepted!");
        //
        // byte[] buffer = new byte[256];
        // clientSocket.Receive(buffer);
        // var receivedMessage = Encoding.UTF8.GetString(buffer).Trim();
        //
        // MessageAccepted?.Invoke(this, new MessageAcceptedEventArgs(receivedMessage));
    }

    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void HandleSocketConnection(IAsyncResult result)
    {
        Dictionary<string, object> storage = (Dictionary<string, object>) result.AsyncState;

        TcpListener tcpListener = (TcpListener)storage["listener"];

        // End the operation and display the received data on the
        //console.
        Socket clientSocket = tcpListener.EndAcceptSocket(result);

        byte[] buffer = new byte[256];
        clientSocket.Receive(buffer);
        var receivedMessage = Encoding.UTF8.GetString(buffer).Trim();
        Debug.Log($"received message: {receivedMessage}");
        
        tcpListener.BeginAcceptSocket(new AsyncCallback(HandleSocketConnection), tcpListener);
        
        ScheduleTask(new FrameDescriptionTask(receivedMessage.TrimEnd('\0')));
    }
    
    void Update () {
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
    }
 
    private void ScheduleTask(ITask newTask)
    {
        lock (_queueLock)
        {
            _taskQueue.Enqueue(newTask);
        }
    }
}