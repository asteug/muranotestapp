using System;
using System.Collections.Generic;
using System.Linq;
using AirportProxyService.Models;

namespace AirportProxyService.Helpers
{
    public class StaticClass
    {
        public static List<Route> ListOfChain { get; set; } = new List<Route>();
        public static bool GetChain(List<RouteEntry> routes, string source, string destination)
        {
            var dest = routes.Find(f => f.Routes.Exists(s => s.DestAirport == destination));
            if (dest != null && dest.Routes.Any(a=>a.DestAirport == destination))
            {
                var route = dest.Routes.FirstOrDefault(a => a.DestAirport == destination);
                if (ListOfChain == null || ListOfChain.Count == 0)
                {
                    ListOfChain = new List<Route>();
                    ListOfChain.Add(route);
                }

                if (dest.Parent != null || route.SrcAirport != source)
                {
                    ListOfChain.Add(dest.Parent);
                    return GetChain(routes, source, dest.ElementName);
                }
            }

            return true;

        }
    }
}