using System;
using AirportProxyService.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirportProxyService.Converters
{
    public class ExceptionResultConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AirportProxyException);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            try
            {
                if (array[0].ToString() == "error")
                {

                    return new AirportProxyException
                    {
                        ErrorCode = (int)array[1],
                        Message = (string)array[2]
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}