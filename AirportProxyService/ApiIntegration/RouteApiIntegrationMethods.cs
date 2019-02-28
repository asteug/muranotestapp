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
    /// Integration with 3-rd party service, which gets routes by query
    /// </summary>
    public class RouteApiIntegrationMethods : ICommonParameters
    {
        public string CallUrl => $"{ConfigurationManager.AppSettings["linkForApi"]}/api/Route/outgoing";
        private RestClient _client;

        public RouteApiIntegrationMethods()
        {
            _client = new RestClient(CallUrl);
        }
        /// <summary>
        /// Searches for all routes from a given airport.
        /// </summary>
        /// <param name="airport">string 3 letter airport alias</param>
        /// <returns></returns>
        public async Task<CommonMessage<List<Route>>> GetAllRoutesAsync(string airport, CancellationToken token)
        {
            try
            {
                var  request = new RestRequest();
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Accept", "text/plain");
                request.Method = Method.GET;
                request.AddParameter("airport", airport);
                var response = await _client.ExecuteTaskAsync(request, token);
                if (!IsNullOrEmpty(response.ErrorMessage)) throw new Exception(response.ErrorMessage);
                var content = Deserializers.DeserializeJson.DeserializeObject<List<Route>>(response.Content, new ExceptionResultConverter());
                if(content is List<Route>)
                    return  new CommonMessage<List<Route>>("Success", true, content as List<Route>);
                return new CommonMessage<List<Route>>("Error", false, new List<Route>());
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, $"GetAllRoutesAsync exception handler\r\n {ex.Message} \r\n Inner exception: {ex.InnerException?.Message}");
                return new CommonMessage<List<Route>>(null, ex.Message);
            }
        }

    }
}