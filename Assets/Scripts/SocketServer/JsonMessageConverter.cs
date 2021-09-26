using System;
using SocketServer.Decorator;
using UnityEngine;

namespace SocketServer
{
    public class JsonMessageConverter<TInDto, TOutDto> : IHandler<string, string>
    {
        private readonly IHandler<TInDto, TOutDto> _handler;
        private readonly TOutDto _nullObject;

        public JsonMessageConverter(IHandler<TInDto, TOutDto> handler, TOutDto nullObject)
        {
            _handler = handler;
            _nullObject = nullObject;
        }

        public string Handle(string input)
        {
            try {
                Debug.Log($"[JsonMessageConverter] input: {input}");
                
                var inputDto = JsonUtility.FromJson<TInDto>(input);
                
                var outputDto = _handler.Handle(inputDto);

                var outputJson = JsonUtility.ToJson(new JsonOutputMessage<TOutDto>(true, outputDto));
                
                Debug.Log($"[{GetType().Name}] output: {outputJson}");

                return outputJson;
            } catch (Exception exception) {
                Debug.Log($"[JsonMessageConverter] exception: {exception.Message}");
                
                return JsonUtility.ToJson(new JsonOutputMessage<TOutDto>(
                    false, 
                    _nullObject, 
                    1,
                    exception.Message)
                );
            }
        }
    }
}