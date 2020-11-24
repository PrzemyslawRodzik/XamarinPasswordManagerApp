using PasswordManagerMobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasswordManagerMobile.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(int id);
        bool Clear();
        Task<T> GetItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<SharedLoginModel>> GetSharedItems(bool forceRefresh = false);

    }
}
