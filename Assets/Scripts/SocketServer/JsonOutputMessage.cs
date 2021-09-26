namespace SocketServer
{
    [System.Serializable]
    public class JsonOutputMessage<T>
    {
        public bool status;
        public T result;
        public int errorCode;
        public string errorMessage;

        public JsonOutputMessage(bool status, T result, int errorCode = 0, string errorMessage = "")
        {
            this.status = status;
            this.result = result;
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
        }
    }
}