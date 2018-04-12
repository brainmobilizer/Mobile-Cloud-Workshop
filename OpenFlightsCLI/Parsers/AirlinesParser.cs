using System;
using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using OpenFlightsCLI.Models;
using OpenFlightsCLI.Models.Raw;

namespace OpenFlightsCLI.Parsers
{
    public class AirlinesParser
    {
        
        string searchServiceName;
        string searchServiceAdminApiKey;

        public AirlinesParser()
        {
            
        }

        public AirlinesParser(string searchServiceName, string searchServiceAdminApiKey)
        {
            this.searchServiceName = searchServiceName;
            this.searchServiceAdminApiKey = searchServiceAdminApiKey;
        }

        public List<Airline> Parse()
        {
            var engine = new FileHelperEngine<AirlineRaw>();
            engine.ErrorManager.ErrorMode = ErrorMode.IgnoreAndContinue;

            var result = engine.ReadFile("Data/Airlines.dat");
            List<Airline> airlines = new List<Airline>();

            foreach (var airlineRaw in result)
            {
                if (!string.IsNullOrEmpty(airlineRaw.Name))
                {
                    var airline = new Airline
                    {
                        Id = airlineRaw.Id.ToString(),
                        Name = airlineRaw.Name,
                        CallSign = airlineRaw.CallSign,
                        AlternativeName = airlineRaw.AlternativeName,
                        Country = airlineRaw.Country,
                        IATA = airlineRaw.IATA,
                        ICAO = airlineRaw.ICAO
                    };
                    airlines.Add(airline);
                }
            }
            return airlines;
            //PushToSearchIndex(airlines);
        }

        void PushToSearchIndex(List<Airline> airlines)
        {
            var serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(searchServiceAdminApiKey));

            if (serviceClient.Indexes.Exists("airlines"))
            {
                serviceClient.Indexes.Delete("airlines");
            }

            var definition = new Index()
            {
                Name = "airlines",
                Fields = FieldBuilder.BuildForType<Airline>()
            };


            serviceClient.Indexes.Create(definition);
            ISearchIndexClient indexClient = serviceClient.Indexes.GetClient("airlines");

            if(airlines.Count > 0)
            {
                while (airlines.Count > 0)
                {
                    var toUpdate = airlines.Take(999);
                    airlines.RemoveRange(0, toUpdate.Count());
                    var batch = IndexBatch.Upload(toUpdate);
                    if (batch.Actions.Count() == 0)
                        break;
                    
                    try
                    {
                        Console.WriteLine(batch.Actions.Count());

                        indexClient.Documents.Index(batch);
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


                Console.WriteLine($"Uploading {airlines.Count()} Airlines to Azure Search");
            }
        }
    }
}
