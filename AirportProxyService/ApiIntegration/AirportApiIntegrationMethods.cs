using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using AirportProxyService.Converters;
using AirportProxyService.Models;
using Serilog;
using RestSharp;
using static System.String;


namespace AirportProxyService.ApiIntegration
{
    /// <summary>
    /// Integration with 3-rd party service, which gets airport by query
    /// </summary>
    public class AirportApiIntegrationMethods : ICommonParameters
    {
        public string CallUrl => $"{ConfigurationManager.AppSettings["linkForApi"]}/api/Airport/search";
        private RestClient _client;

        public AirportApiIntegrationMethods()
        {
            _client = new RestClient(CallUrl);
        }
        
        /// <summary>
        /// Searches for airports by a provided pattern.
        /// </summary>
        /// <param name="pattern">string 3 letter airport</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        public async Task<CommonMessage<List<Airport>>> GetAllAirportByPatternAsync(string pattern, CancellationToken token)
        {
            try
            {
                var  request = new RestRequest();
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Accept", "text/plain");
                request.Method = Method.GET;
                request.AddParameter("pattern", pattern);
                var response = await _client.ExecuteTaskAsync(request, token);
                if (!IsNullOrEmpty(response.ErrorMessage)) throw new Exception(response.ErrorMessage);
                var content = Deserializers.DeserializeJson.DeserializeObject<List<Airport>>(response.Content, new ExceptionResultConverter());
                if(content is List<Airport>)
                    return  new CommonMessage<List<Airport>>("Success", true, content as List<Airport>);
                return new CommonMessage<List<Airport>>("Error", false, new List<Airport>());
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, $"GetAllRoutesAsync exception handler\r\n {ex.Message} \r\n Inner exception: {ex.InnerException?.Message}");
                return new CommonMessage<List<Airport>>(null, ex.Message);
            }
        }

    }
}