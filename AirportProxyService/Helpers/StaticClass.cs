using System;
using System.Collections.Generic;
using System.Linq;
using AirportProxyService.Models;

namespace AirportProxyService.Helpers
{
    public static class StaticClass
    {
         
        public static List<Route> ListOfChain { get; set; } = new List<Route>();
        public static bool GetChain(List<RouteEntry> routes, string source, string destination)
        {
            var dest = routes.Find(f => f.Routes.Exists(s => s.DestAirport == destination));
            if (dest != null && dest.Routes.Any(a=>a.DestAirport == destination))
            {
                if (ListOfChain == null || ListOfChain.Count == 0)
                {
                    ListOfChain = new List<Route>(){new Route(){}};
                    ListOfChain.Add(new Tuple<string, string, string>());
                }
                ListOfChain.Add(dest.ElementName);
                if(dest.Parent!=null)
                    return GetChain(routes, source, dest.ElementName);

            }

            return true;

        }
    }
}