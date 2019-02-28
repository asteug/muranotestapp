namespace AirportProxyService.Models
{
    public class RouteExtension 
    {
        public string Source { get; set; }
        public string Destination { get; set; }

        public string Airline { get; set; }
        public bool IsWorkingAirline { get; set; }
        public bool IsParent { get; set; }

        public RouteExtension RouteExtensionValue { get; set; }

    }
}