using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AirportProxyService.Models
{
    public class SearchDestinationModel
    {
        [JsonProperty("fromAirport")]
        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string FromAirport { get; set; }

        [JsonProperty("fromCity")]
        [Required]
        public string FromCity { get; set; }



        [JsonProperty("toAirport")]
        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string ToAirport { get; set; }

        [JsonProperty("toCity")]
        [Required]
        public string ToCity { get; set; }
    }
}