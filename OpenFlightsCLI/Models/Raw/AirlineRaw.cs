using System;
using FileHelpers;

namespace OpenFlightsCLI.Models.Raw
{
    [DelimitedRecord(",")]
    public class AirlineRaw
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AlternativeName { get; set; }
        public string IATA { get; set; }
        public string ICAO { get; set; }
        public string CallSign { get; set; }
        public string Country { get; set; }
        public string Active { get; set; }
    }
}
