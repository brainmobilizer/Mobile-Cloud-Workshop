using System;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;

namespace OpenFlightsCLI.Models
{

    public interface ISearchBaseModel 
    {
        string Id { get; set; }
    }

    [SerializePropertyNamesAsCamelCase]
    public class SearchBaseModel : ISearchBaseModel
    {
        [JsonProperty("id")]
        [System.ComponentModel.DataAnnotations.Key]
        public string Id { get; set; }

    }
}
