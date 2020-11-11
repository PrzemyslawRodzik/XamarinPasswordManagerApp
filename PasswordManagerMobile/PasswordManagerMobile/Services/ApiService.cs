
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PasswordManagerMobile.ApiResponses;
using PasswordManagerMobile.Configuration;
using PasswordManagerMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PasswordManagerApp.Services
{
    public class ApiService
    {

        HttpClient _client;
        
        
        
        public ApiService()
         {
            
            _client = new HttpClient(new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            }
            });
            _client.BaseAddress = new Uri("https://192.168.1.100:5006/api/");
           // _client.BaseAddress = new Uri("https://10.0.2.2:5006/api/");
            _client.DefaultRequestHeaders.Authorization = GetAuthJwtTokenFromKeyStore().Result;
            
            
        }

        private async Task<AuthenticationHeaderValue> GetAuthJwtTokenFromKeyStore()
        {

            var jwtToken = await SecureStorage.GetAsync(StorageConstants.AccessToken);
            

            return new AuthenticationHeaderValue("Bearer", jwtToken);
        }
           
           



        public  bool CheckEmailAvailability(string email)
        {
            
            var response = _client.GetAsync($"users/email/check?email={email}").Result;

            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<Dictionary<string,int>> GetUserStatisticData()
        {
            
            var response = await _client.GetAsync("statistics/user-data");
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Dictionary<string,int>>(responseString);
        }

        #region Generic CRUD api calls
        public async Task<T> CreateUpdateData<T>(T entity, int? id = null) where T : class
        {
            HttpResponseMessage response;
            string typeName = typeof(T).Name;
            typeName += "s";
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            if (id == null)
                response = await _client.PostAsync($"{typeName}/", content);
            else
                response = await _client.PutAsync($"{typeName}/{id}", content);

            response.EnsureSuccessStatusCode();
            

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseString);
        }
        public async Task<ApiResponse> HandleLoginShare(ShareLoginModel model)
        {
            HttpResponseMessage response;
            
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            
            response = await _client.PostAsync("logindatas/share", content);
           

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponse>(responseString);
        }
        public async Task<bool> DeleteData<T>(int id)
        {
            HttpResponseMessage response;
            string typeName = typeof(T).Name;
            typeName += "s";

            response = await _client.DeleteAsync($"{typeName}/{id}");

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;




        }

        public async Task<IEnumerable<T>> GetAllUserData<T>(int userId, int? compromised = null) where T : class
        {
            string typeName = typeof(T).Name;
            typeName += "s";
            var response =  _client.GetAsync($"{typeName}?userId={userId}").Result;
            
            response.EnsureSuccessStatusCode();

            var responseString =  response.Content.ReadAsStringAsync().Result;
                
            return  JsonConvert.DeserializeObject<IEnumerable<T>>(responseString);

        }

        public  bool CheckUserGuidDeviceInDb(string guidDeviceHash,int userId)
        {
            HttpResponseMessage response;
            StringContent content = new StringContent(JsonConvert.SerializeObject(new {GuidDevice = guidDeviceHash, UserId = userId }), Encoding.UTF8, "application/json");
            response = _client.PostAsync("users/devices/check-guid", content).Result;
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        

        public void HandleNewDeviceLogIn(string ipAddress, string guidDevice,int userId, string osName, string browserName)
        {
            HttpResponseMessage response;
            StringContent content = new StringContent(JsonConvert.SerializeObject(new { IpAddress = ipAddress, GuidDevice = guidDevice, UserId = userId, OSName = osName, BrowserName = browserName }), Encoding.UTF8, "application/json");
            response = _client.PostAsync("users/devices/authorize-new-device", content).Result;
           
        }

        public async Task<T> GetDataById<T>(int id) where T : class
        {
            string typeName = typeof(T).Name;
            typeName += "s";
            var response = await _client.GetAsync($"{typeName}/{id}");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseString);
        }

       

        

        

        

        

       

        #endregion

        #region Authentication api calls

        

        public async Task<AuthResponse> LogIn(LoginModel model)
        {
            HttpResponseMessage response;
            
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            try
            {
                response = await _client.PostAsync($"identity/login", content);
                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<AuthResponse>(responseString);
            }
            catch (Exception)
            {
                return new AuthResponse
                {
                    Success = false,
                    Messages = new string[] { "There was an error. Check your internet connection." }
                };
            }
            

            


        }

        public User GetAuthUser()
        {

            var response = _client.GetAsync("users/getauthuser").Result;
            
            if (response.StatusCode.Equals(StatusCodes.Status404NotFound))
                return null;

            var responseString = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<User>(responseString);
        }
        

        public async Task<ApiTwoFactorResponse> TwoFactorLogIn(int userId, string token)
        {
            
            HttpResponseMessage response;
            StringContent content = new StringContent(JsonConvert.SerializeObject(new {userId = userId, token = token }) , Encoding.UTF8, "application/json");
            response = await _client.PostAsync($"identity/twofactorlogin", content);
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiTwoFactorResponse>(responseString);
        }

        public async Task<ApiResponse>  SendTotpByEmail(int idUser)
        {
            HttpResponseMessage response;
            StringContent content = new StringContent(JsonConvert.SerializeObject(idUser), Encoding.UTF8, "application/json");
            response = await _client.PostAsync($"identity/twofactorlogin/resendtotp", content);
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponse>(responseString);
        }

        #endregion
    }
}
