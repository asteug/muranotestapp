using Newtonsoft.Json;

namespace AirportProxyService.Models
{
    public class Route
    {
        /// <summary>
        /// 2-letter (IATA) or 3-letter (ICAO) code of the airline
        /// </summary>
        [JsonProperty("airline")]
        public string Airline { get; set; }

        /// <summary>
        /// 3-letter(IATA) or 4-letter(ICAO) code of the source airport.
        /// </summary>
        [JsonProperty("srcAirport")]
        public string SrcAirport { get; set; }

        /// <summary>
        /// 3-letter (IATA) or 4-letter (ICAO) code of the destination airport.
        /// </summary>
        [JsonProperty("destAirport")]
        public string DestAirport { get; set; }
    }
}