using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FileHelpers;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using OpenFlightsCLI.Models;
using OpenFlightsCLI.Models.Raw;

namespace OpenFlightsCLI.Parsers
{
    public class AirportsParser
    {
        string searchServiceName;
        string searchServiceAdminApiKey;

        public AirportsParser(string searchServiceName, string searchServiceAdminApiKey)
        {
            this.searchServiceName = searchServiceName;
            this.searchServiceAdminApiKey = searchServiceAdminApiKey;
        }

        public void Parse()
        {
            var engine = new FileHelperEngine<AirportRaw>();
            engine.ErrorManager.ErrorMode = ErrorMode.IgnoreAndContinue;

            Console.WriteLine("Pass in the Airports.dat file found the Data directory");

            var result = engine.ReadFile(Console.ReadLine().Trim());
            List<Airport> airports = new List<Airport>();

            foreach (var rawAirport in result)
            {
                if (!string.IsNullOrEmpty(rawAirport.Name))
                {
                    Console.WriteLine($"Name: {rawAirport.Name}");
                    Console.WriteLine($"Code: {rawAirport.Code}");
                    Console.WriteLine($"Country: {rawAirport.Country}");

                    Console.WriteLine("");

                    var airport = new Airport();
                    airport.Id = rawAirport.Id.ToString();
                    airport.Name = rawAirport.Name;
                    airport.City = rawAirport.City;
                    airport.Code = rawAirport.Code;
                    airport.Country = rawAirport.Country;
                    airport.ICAO = rawAirport.ICAO;
                    airport.Latitude = rawAirport.Latitude;
                    airport.Longitude = rawAirport.Longitude;

                    airports.Add(airport);
                }
            }



            Console.WriteLine("");
            Console.WriteLine($"Total Airports: {result.Count()}");
            Console.WriteLine($"------------------------------------------------------------");
            Console.WriteLine("");

            PushToSearchIndex(airports);
            Console.WriteLine("");
        }

        void PushToSearchIndex(List<Airport> airports)
        {
            Console.WriteLine($"Add {airports.Count()} airports to the index");
            var serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(searchServiceAdminApiKey));

            if (serviceClient.Indexes.Exists("airports"))
            {
                Console.WriteLine("Deleted existing Index");
                serviceClient.Indexes.Delete("airports");
            }

            var definition = new Index()
            {
                Name = "airports",
                Fields = FieldBuilder.BuildForType<Airport>()
            };



            serviceClient.Indexes.Create(definition);
            ISearchIndexClient indexClient = serviceClient.Indexes.GetClient("airports");

            if (airports.Count > 0)
            {
                while (airports.Count > 0)
                {
                    var toUpdate = airports.Take(999);
                    airports.RemoveRange(0, toUpdate.Count());
                    var batch = IndexBatch.Upload(toUpdate);
                    if (batch.Actions.Count() == 0)
                        break;

                    try
                    {
                        Console.WriteLine(batch.Actions.Count());
                        indexClient.Documents.Index(batch);
                        Thread.Sleep(10);

                    }
                    catch (IndexBatchException e)
                    {
                        // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                        // the batch. Depending on your application, you can take compensating actions like delaying and
                        // retrying. For this simple demo, we just log the failed document keys and continue.
                        Console.WriteLine(
                            "Failed to index some of the documents: {0}",
                            String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
                    }
                }


                Console.WriteLine($"Uploading {airports.Count()} Airports to Azure Search");
            }
        }
    }
}
