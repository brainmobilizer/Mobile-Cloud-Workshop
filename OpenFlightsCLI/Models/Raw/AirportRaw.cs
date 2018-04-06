using System;
using FileHelpers;

namespace OpenFlightsCLI.Models.Raw
{
    [DelimitedRecord(",")]
    public class AirportRaw
    {
        //507
        public int Id { get; set; }

        //London Heathrow Airport
        public string Name { get; set; }


        //London
        public string City { get; set; }

        //United Kingdom
        public string Country { get; set; }

        //LHR
        public string Code { get; set; }

        //EGLL
        public string ICAO { get; set; }

        //51.4706
        public string Latitude { get; set; }

        //-0.461941
        public string Longitude { get; set; }

        //83
        public string Elevation { get; set; }

        //0
        public string UTC { get; set; }

        //E
        public string DST { get; set; }

        //Europe/London
        public string Unknown { get; set; }

        //airport
        public string Type { get; set; }

        //OurAirports
        public string Database { get; set; }


    }
}
