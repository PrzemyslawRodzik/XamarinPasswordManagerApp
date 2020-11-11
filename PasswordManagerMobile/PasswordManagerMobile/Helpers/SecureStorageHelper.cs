using PasswordManagerApp.Services;
using PasswordManagerMobile.ApiResponses;
using PasswordManagerMobile.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PasswordManagerMobile.Helpers
{
   public class SecureStorageHelper
    {


        public static async Task<Task> SaveUserData(AuthResponse authResponse, string password,string email)
        {
            await SecureStorage.SetAsync(StorageConstants.AccessToken, authResponse.AccessToken.JwtToken);
            await SecureStorage.SetAsync(StorageConstants.AccessTokenExpireTime, authResponse.AccessToken.Expire.ToString());
            await SecureStorage.SetAsync(StorageConstants.UserPassword,password);
            
            Application.Current.Properties[StorageConstants.Email] = email;
            await Application.Current.SavePropertiesAsync();
            return Task.CompletedTask;

        }
        public static async Task<Task> SaveUserData(AuthResponse authResponse,string userInfo, string password,string email)
        {
            await SecureStorage.SetAsync(StorageConstants.AccessToken, authResponse.AccessToken.JwtToken);
            await SecureStorage.SetAsync(StorageConstants.AccessTokenExpireTime, authResponse.AccessToken.Expire.ToString());
            await SecureStorage.SetAsync(StorageConstants.UserId, userInfo);
            await SecureStorage.SetAsync(StorageConstants.UserPassword, password);
            await SaveUserEmail(email);
            return Task.CompletedTask;

        }
        public static async Task<Task> SaveUserData(AuthResponse authResponse)
        {
            await SecureStorage.SetAsync(StorageConstants.AccessToken, authResponse.AccessToken.JwtToken);
            await SecureStorage.SetAsync(StorageConstants.AccessTokenExpireTime, authResponse.AccessToken.Expire.ToString());
            return Task.CompletedTask;

        }
        public static async  Task<Task> SaveUserEmail(string email)
        {
            Application.Current.Properties[StorageConstants.Email] = email;
            await Application.Current.SavePropertiesAsync();
            return Task.CompletedTask;

        }
        

        public static async Task<int> GetUserId()
        {
            var idUser = await SecureStorage.GetAsync(StorageConstants.UserId);
            return Int32.Parse(idUser);
        }
        public static string GetUserEmail()
        {
            if (Application.Current.Properties.ContainsKey(StorageConstants.Email))
                return Application.Current.Properties[StorageConstants.Email].ToString();
            return "Not added";
            
        }
        public static async Task<string> GetUserKey()
        {
            var key = await SecureStorage.GetAsync(StorageConstants.UserPassword);
            return key;
        }


        public static async Task<bool> CheckIfUserSessionIsActive()
        {
            try
            {
                var authToken = await SecureStorage.GetAsync(StorageConstants.AccessToken);
                if (string.IsNullOrEmpty(authToken))
                    return false;
                var tokenExpireTime = await SecureStorage.GetAsync(StorageConstants.AccessTokenExpireTime);
                var tokenExpireDate = DateTime.Parse(tokenExpireTime);
                if (tokenExpireDate < DateTime.UtcNow)
                    return false;
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            

        }



        public static void ClearData()
        {
            SecureStorage.RemoveAll();
            Application.Current.Resources.Clear();
        }
    }
}
