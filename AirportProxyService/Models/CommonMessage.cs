using Newtonsoft.Json;

namespace AirportProxyService.Models
{
    public class CommonMessage
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccessful { get;}
        [JsonProperty("message")]
        public string Message { get; }

        public CommonMessage(string message, bool isSuccessful)
        {
            Message = message;
            IsSuccessful = isSuccessful;
        }
    }


    public class CommonMessage<T> : CommonMessage
    {
        public T Result { get; }

        public CommonMessage(string message, bool isSuccessful, T result) : base(message, isSuccessful)
        {
            Result = result;
        }

        public CommonMessage(T result, string message) : base(message, result != null)
        {
            Result = result;
        }

    }
}