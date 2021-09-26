using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SocketServer.Decorator;
using UnityEngine;

namespace SocketServer
{
    public class Server
    {
        private readonly IHandler<string, string> _handler;

        public Server(IHandler<string, string> handler)
        {
            _handler = handler;
        }
        
        public void Run()
        {
            const int port = 8110;
            var ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipPoint);
        
            listener.Listen(100);

            while (true) {
                try {
                    var socket = listener.Accept();
                    
                    var builder = new StringBuilder();
                    do {
                        var data = new byte[256];
                        var bytes = socket.Receive(data);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    } while (socket.Available > 0);

                    var request = builder.ToString();

                    try {
                        var response = _handler.Handle(request);
                        SendResponse(socket, response);
                    } catch (Exception exception) {
                        SendResponse(socket, exception.Message);    
                    }
                } catch (Exception exception) { // TODO: clarify for SocketException and ObjectDisposedException
                    Debug.Log($"From server: {exception.Message}");
                }
            }
        }
        
        private void SendResponse(Socket socket, string response)
        {
            var data = Encoding.UTF8.GetBytes(response);
            socket.Send(data);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
