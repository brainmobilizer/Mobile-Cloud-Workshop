using System;
using FileHelpers;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;

namespace OpenFlightsCLI.Models
{
    [SerializePropertyNamesAsCamelCase]
    public class Airport
    {
        [JsonProperty("id")]
        [System.ComponentModel.DataAnnotations.Key]
        public string Id { get; set; }

        [IsSearchable]
        public string Name { get; set; }

        [IsFilterable]
        public string City { get; set; }

        [IsFilterable]
        public string Country { get; set; }

        [IsFilterable, IsSearchable]
        public string Code { get; set; }

        public string ICAO { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

    }
}
