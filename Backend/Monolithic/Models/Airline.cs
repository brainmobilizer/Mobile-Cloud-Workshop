using System;
namespace ContosoMaintenance.WebAPI.Models
{
    public class Airline
    {
        public string Name { get; set; }
        public string IATA { get; set; }
        public string ICAO { get; set; }
        public string Country { get; set; }
        public string CallSign { get; set; }
    }
}
