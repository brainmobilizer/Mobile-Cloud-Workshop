using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using OpenFlightsCLI.Models;

namespace OpenFlightsCLI.Helpers
{
    public class AzureSearchServiceBase<T> where T : class
    {
        SearchServiceClient serviceClient;

        public AzureSearchServiceBase(string instanceName, string apiKey)
        {
            serviceClient = new SearchServiceClient(instanceName, new SearchCredentials(apiKey));
        }

        public bool CreateIndexOfType<T>(bool overrideExisting = true) 
        {
            var indexName = IndexNameForType<T>();

            if (serviceClient.Indexes.Exists(indexName))
            {
                if (overrideExisting)
                    serviceClient.Indexes.Delete(indexName);
                else
                    return false;
            }

            var definition = new Index()
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<T>()
            };

            serviceClient.Indexes.Create(definition);
            var indexClient = serviceClient.Indexes.GetClient(indexName);

            if (indexClient != null)
                return true;
            return false;

        }

        public bool UploadDocuments(List<T> documents)
        {
            var indexName = IndexNameForType<T>();
            var indexClient = serviceClient.Indexes.GetClient(indexName);

            if (documents.Count > 0)
            {
                while (documents.Count > 0)
                {
                    var toUpdate = documents.Take(999);
                    documents.RemoveRange(0, toUpdate.Count());

                    var batch = IndexBatch.Upload(toUpdate);
                    if (batch.Actions.Count() == 0)
                        break;

                    try
                    {
                        indexClient.Documents.Index(batch);
                        Thread.Sleep(10);

                    }
                    catch (IndexBatchException e)
                    {
                        return false;
                    }
                }

                return true;
            }
            return false;

        }

        public bool WorkingConnection()
        {
            var apiVersion = serviceClient.ApiVersion;

            if (string.IsNullOrEmpty(apiVersion))
                return false;

            return true;

        }

        public bool UploadAirports(List<Airport> airports)
        {
            var indexName = IndexNameForType<Airport>();
            CreateIndexOfType<Airport>(false);

            var indexClient = serviceClient.Indexes.GetClient(indexName);

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
                        indexClient.Documents.Index(batch);
                        Thread.Sleep(10);

                    }
                    catch (IndexBatchException e)
                    {
                        return false;
                    }
                }

                return true;
            }
            return false;
        }

        public bool UploadAirlines(List<Airline> airlines)
        {
            var indexName = IndexNameForType<Airline>();
            CreateIndexOfType<Airline>(false);

            var indexClient = serviceClient.Indexes.GetClient(indexName);

            if (airlines.Count > 0)
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
                        indexClient.Documents.Index(batch);
                        Thread.Sleep(10);

                    }
                    catch (IndexBatchException e)
                    {
                        return false;
                    }
                }

                return true;
            }
            return false;
        }

        public bool IndexExistsForType<T>()
        {
            var indexName = IndexNameForType<T>();
            return serviceClient.Indexes.Exists(indexName);
        }

        string IndexNameForType<T>()
        {
            var name = typeof(T).Name.ToLower();
            return name + "s";
        }

        public int Count<T>()
        {
            var indexName = IndexNameForType<T>();
            var indexClient = serviceClient.Indexes.GetClient(indexName);
            var parameters = new SearchParameters();
            //  var results =  indexClient.Documents.Search<T>("", parameters);

            //  return (int)results.Count;
            return 0;
        }
    }
}
