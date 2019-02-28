using Newtonsoft.Json;
using Serilog;

namespace AirportProxyService.Deserializers
{
    public static class DeserializeJson
    {
        #region private
        /// <summary>
        /// Deserializes JSON to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static object DeserializeObject<T>(string json, JsonConverter converter)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, converter);
            }
            catch (JsonReaderException ex)
            {
                Log.Logger.Error(ex, $"Error deserializing response \r\nMessage: {ex.Message} \r\nInner Message: {ex.InnerException?.Message}");
                return null;
            }
        }
        #endregion
    }
}