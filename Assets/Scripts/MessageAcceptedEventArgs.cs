using System;

public class MessageAcceptedEventArgs : EventArgs
{
    public string Message { get; set; }

    public MessageAcceptedEventArgs(string message)
    {
        Message = message;
    }
}
