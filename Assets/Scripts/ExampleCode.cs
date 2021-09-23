using System.Net.Sockets;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;

public class ExampleCode : MonoBehaviour
{
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

        TcpListener listener = TcpListener.Create(8110);
        listener.Start();
        listener.BeginAcceptSocket(new AsyncCallback(handleSocketConnection), listener);

        Debug.Log("Started async socket server on 8110...");
    }

    // called third
    void Start()
    {
        Debug.Log("Start");
    }

    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void handleSocketConnection(IAsyncResult result)
    {
        // Get the listener that handles the client request.
        TcpListener tcpListener = (TcpListener)result.AsyncState;

        // End the operation and display the received data on the
        //console.
        Socket clientSocket = tcpListener.EndAcceptSocket(result);

        byte[] buffer = new byte[256];
        clientSocket.Receive(buffer);
        Debug.Log(Encoding.UTF8.GetString(buffer));

        tcpListener.BeginAcceptSocket(new AsyncCallback(handleSocketConnection), tcpListener);
    }
}