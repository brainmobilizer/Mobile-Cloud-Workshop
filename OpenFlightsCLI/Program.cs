using System;
using System.Linq;
using FileHelpers;
using OpenFlightsCLI.Models;

namespace OpenFlightsCLI
{
    class Program
    {
        static string azureSearchUrl { get; set; }
        static string azureSearchKey { get; set; }

        static void Main(string[] args)
        {

#if DEBUG
            Console.WriteLine("Get cocking started!");

            azureSearchKey = "5F01767846A092CB8678973CA05D5B60";
            azureSearchUrl = "contosomaintenance";

            var airlineParser2 = new Parsers.AirportsParser(azureSearchUrl, azureSearchKey);
            airlineParser2.Parse();
#endif


            Console.WriteLine("Search Builder 0.1");
            Console.WriteLine("");

            Console.WriteLine("Azure Search Name");
            azureSearchUrl = Console.ReadLine().Trim();

            Console.WriteLine("");
            Console.WriteLine("Azure Search API Key:");
            azureSearchKey = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine("Would you like to import Airlines or Airports?");
            var requestType = Console.ReadLine().Trim().ToLower();

            if(requestType == "airlines")
            {
                var airlineParser = new Parsers.AirlinesParser(azureSearchUrl, azureSearchKey );
                airlineParser.Parse();
            }
            if (requestType == "airports")
            {
                var airportsParser = new Parsers.AirportsParser(azureSearchUrl, azureSearchKey);
                airportsParser.Parse();
            }

            Console.WriteLine("Would you like another go?");
            var anotherGo = Console.ReadLine().Trim();

            if (anotherGo == "no")
            {
                return;
            }
            else
            {
                Console.WriteLine("Would you like to import Airlines or Airports?");
                requestType = Console.ReadLine().Trim().ToLower();



                if (requestType == "airlines")
                {
                    var airlineParser = new Parsers.AirlinesParser(azureSearchUrl, azureSearchKey);
                    airlineParser.Parse();
                }
                if (requestType == "airports")
                {
                    var airportsParser = new Parsers.AirportsParser(azureSearchUrl, azureSearchKey);
                    airportsParser.Parse();
                }
            }
            Console.ReadLine();


        }
    }
}
