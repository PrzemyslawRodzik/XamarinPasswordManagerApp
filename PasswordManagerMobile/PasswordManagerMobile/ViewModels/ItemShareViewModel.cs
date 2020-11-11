using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PasswordGenerator;
using PasswordManagerApp.Handlers;
using PasswordManagerApp.Services;
using PasswordManagerMobile.Helpers;
using PasswordManagerMobile.Models;
using Xamarin.Forms;

namespace PasswordManagerMobile.ViewModels
{
    
    public class ItemShareViewModel : BaseViewModel
    {
        private string expireDate;
        private string receiverEmail;

        private readonly AppSettings _config;
        private readonly ApiService _apiService;

        public Command ShareCommand { get; }
        public Command CancelCommand { get; }
        

        public ItemShareViewModel(int itemId)
        {
            _apiService = new ApiService();
            _config = App.AppSettings;
            ItemId = itemId;
            ShareCommand = new Command(OnShare, ValidateShare);
            this.PropertyChanged +=
                 (_, __) => ShareCommand.ChangeCanExecute();
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                 (_, __) => CancelCommand.ChangeCanExecute();
            



            
        }

        

        private bool ValidateShare()
        {
            return !String.IsNullOrWhiteSpace(receiverEmail)
                && !String.IsNullOrWhiteSpace(expireDate)
                && !IsBusy;



        }
        private void OnCancel()
        {
             App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void OnShare()
        {
            IsBusy = true;
            var loginDataToShare = await DataStore.GetItemAsync(ItemId);
            var password = EncService.Decrypt(SecureStorageHelper.GetUserKey().Result, loginDataToShare.Password);
            LoginData newLoginData = new LoginData()
            {   
                Name = loginDataToShare.Name+" From "+SecureStorageHelper.GetUserEmail(),
                Login = loginDataToShare.Login,
                Password = EncService.Encrypt(_config.SecretEncryptionKey, password),
                Website = loginDataToShare.Website,
                Email = loginDataToShare.Email ?? "Not added"
                
                
            };
            var model = new ShareLoginModel
            {
                LoginData = newLoginData,
                ReceiverEmail = ReceiverEmail,
                StartDate = DateTime.UtcNow.ToLocalTime(),
                EndDate = DateTime.UtcNow.AddDays(15).ToLocalTime()
            };
            
            var apiResponse = await _apiService.HandleLoginShare(model);

            IsBusy = false;

            // This will pop the current page off the navigation stack
            await App.Current.MainPage.Navigation.PopModalAsync();
            MessagingCenter.Send(this, "ShareNotify", apiResponse.Messages.First());
            
        }

       
        #region Properties
       

        
        
        
        public string ReceiverEmail
        {
            get => receiverEmail;
            set => SetProperty(ref receiverEmail, value);
        }
        public string ExpireDate
        {
            get => expireDate;
            set => SetProperty(ref expireDate, value);
        }

        public int ItemId { get; set; }
        #endregion



    }
}
