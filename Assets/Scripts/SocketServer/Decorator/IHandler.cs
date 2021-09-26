namespace SocketServer.Decorator
{
    public interface IHandler<in TIn, out TOut>
    {
        TOut Handle(TIn input);
    }
}