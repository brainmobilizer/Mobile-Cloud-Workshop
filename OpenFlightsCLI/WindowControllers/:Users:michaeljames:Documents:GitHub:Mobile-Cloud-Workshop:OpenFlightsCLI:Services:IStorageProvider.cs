using System;
using System.Threading.Tasks;

namespace OpenFlightsCLI.Services
{
    public interface IStorageProvider<T>
    {
        void Intialize(string endpoint, string key, string databaseId);
        void Initialize();

        Task<T> GetItemAsync(string id);

        int GetItemsCount();



    }
}
