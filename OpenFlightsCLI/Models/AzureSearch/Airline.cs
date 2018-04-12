using System;
using FileHelpers;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;

namespace OpenFlightsCLI.Models
{
    [SerializePropertyNamesAsCamelCase]
    public class Airline : SearchBaseModel
    {
        [IsSearchable]
        public string Name { get; set; }

        [IsSearchable]
        public string AlternativeName { get; set; }

        [IsFilterable]
        public string IATA { get; set; }

        [IsFilterable]
        public string ICAO { get; set; }

        [IsSearchable]
        public string CallSign { get; set; }

        [IsFilterable]
        public string Country { get; set; }
    }
}
