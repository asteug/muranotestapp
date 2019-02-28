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
    public class AirlineApiIntegrationMethods : ICommonParameters
    {
        public string CallUrl => $"{ConfigurationManager.AppSettings["linkForApi"]}/api/Airline/";
        private RestClient _client;

        public AirlineApiIntegrationMethods()
        {
            _client = new RestClient(CallUrl);
        }
        
        /// <summary>
        /// Searches for airports by a provided pattern.
        /// </summary>
        /// <param name="pattern">string 3 letter airport</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        public async Task<CommonMessage<List<Airline>>> GetAllAirlineByAliasAsync(string alias, CancellationToken token)
        {
            try
            {
                var  request = new RestRequest();
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Accept", "text/plain");
                request.Method = Method.GET;
                request.Resource += alias;

                var response = await _client.ExecuteTaskAsync(request, token);
                if (!IsNullOrEmpty(response.ErrorMessage)) throw new Exception(response.ErrorMessage);
                var content = Deserializers.DeserializeJson.DeserializeObject<List<Airline>>(response.Content, new ExceptionResultConverter());
                if(content is List<Airline>)
                    return  new CommonMessage<List<Airline>>("Success", true, content as List<Airline>);
                return new CommonMessage<List<Airline>>("Error", false, new List<Airline>());
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, $"GetAllRoutesAsync exception handler\r\n {ex.Message} \r\n Inner exception: {ex.InnerException?.Message}");
                return new CommonMessage<List<Airline>>(null, ex.Message);
            }
        }

    }
}