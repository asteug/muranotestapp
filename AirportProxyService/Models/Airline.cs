using Newtonsoft.Json;

namespace AirportProxyService.Models
{
    public class Airline
    {
        /// <summary>
        /// Name of the airline.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Alias of the airline. For example, All Nippon Airways is commonly known as "ANA".
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }

        /// <summary>
        /// True if the airline is or has until recently been operational, otherwise it is defunct.
        /// </summary>
        [JsonProperty("active")]
        public bool IsActive { get; set; }
    }
}