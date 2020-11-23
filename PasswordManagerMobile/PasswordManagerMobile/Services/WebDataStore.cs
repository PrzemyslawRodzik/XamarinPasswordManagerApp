using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PasswordManagerApp.Services;
using PasswordManagerMobile.Helpers;
using PasswordManagerMobile.Models;

namespace PasswordManagerMobile.Services
{
    public class WebDataStore : IDataStore<LoginData>
    {
        readonly List<LoginData> items;
        readonly ApiService _apiService;

        public WebDataStore()
        {
            _apiService = new ApiService();
            var tempItems = _apiService.GetAllUserData<LoginData>(SecureStorageHelper.GetUserId().Result).Result;
            if(!(tempItems is null) )
                items = tempItems.ToList();



        }
       
        public async Task<LoginData> GetItemAsync(int id)
        {
            var item = items.FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(item);
        }
        public async Task<LoginData> GetSharedItemAsync(int id)
        {
            var item = items.FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(item);
        }


        public async Task<IEnumerable<LoginData>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
             
        }
        public async Task<IEnumerable<SharedLoginModel>> GetSharedItems(bool forceRefresh = false)
        {
            // return await Task.FromResult(items);

            var sharedLogins = _apiService.GetSharedLogins(SecureStorageHelper.GetUserId().Result.ToString()).ToList();
            return await Task.FromResult(sharedLogins);

            

        }




        
        public async Task<bool> AddItemAsync(LoginData item)
        {
            
            await _apiService.CreateUpdateData<LoginData>(item, null);
            items.Add(item);
            return await Task.FromResult(true);
        }
        public async Task<bool> UpdateItemAsync(LoginData item)
        {
            await _apiService.CreateUpdateData<LoginData>(item, item.Id);
            var oldItem = items.Where((LoginData arg) => arg.Id == item.Id).FirstOrDefault();

            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteItemAsync(int id)
        {
            await _apiService.DeleteData<LoginData>(id);
            var oldItem = items.Where((LoginData arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }
    }
}