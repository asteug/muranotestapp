using Newtonsoft.Json;

namespace AirportProxyService.Models
{
    public class Airport
    {
        /// <summary>
        /// Name of airport.May or may not contain the City name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Alias of the airline.For example, All Nippon Airways is commonly known as "ANA".
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Main city served by airport. May be spelled differently from Name.
        /// </summary>
        [JsonProperty("city")]

        public string City { get; set; }

        /// <summary>
        /// Country or territory where airport is located.See countries.dat to cross-reference to ISO 3166-1 codes.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// Decimal degrees, usually to six significant digits. Negative is South, positive is North.
        /// </summary>
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        
        /// <summary>
        /// Decimal degrees, usually to six significant digits. Negative is West, positive is East
        /// </summary>
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// In feet.
        /// </summary>
        [JsonProperty("altitude")]
        public int Altitude { get; set; }
    }
}