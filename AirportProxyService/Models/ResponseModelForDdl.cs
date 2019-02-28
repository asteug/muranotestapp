using System.Collections.Generic;
using Newtonsoft.Json;

namespace AirportProxyService.Models
{
    public class DropDownListItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class ResponseModelForDdl : CommonMessage
    {
        [JsonProperty("items")] public List<DropDownListItem> DropDownListItems { get; set; }

        public ResponseModelForDdl(string message, bool isSuccessful) : base(message, isSuccessful)
        {
        }
    }
}