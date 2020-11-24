using System;
using System.Diagnostics;
using System.Globalization;
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
    
    public class ItemShareAddViewModel : BaseViewModel
    {
        private DateTime expireDate;
        private string receiverEmail;
        private string expireDateResult;
        private TimeSpan selectedTime;

        
        private readonly ApiService _apiService;

        public Command ShareCommand { get; }
        public Command CancelCommand { get; }
        

        public ItemShareAddViewModel(int itemId)
        {
            _apiService = new ApiService();
            ItemId = itemId;
            ShareCommand = new Command(OnShare, ValidateShare);
            this.PropertyChanged +=
                 (_, __) => ShareCommand.ChangeCanExecute();
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                 (_, __) => CancelCommand.ChangeCanExecute();
            ExpireDate = DateTime.UtcNow.ToLocalTime();






        }

        

        private bool ValidateShare()
        {
            
            if (!String.IsNullOrWhiteSpace(receiverEmail)
                && !IsBusy)
            {
                return true;
            }
            return false;
            
            
            



        }
        private void OnCancel()
        {
             App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void OnShare()
        {
            if (!(ReceiverEmail.Contains("@") && ReceiverEmail.Contains(".")))
            {
                EmailError = "Please enter valid email!";
                return;
            }


            var startDate = DateTime.UtcNow.ToLocalTime();
            DateTime endDate;
            try
            {
                endDate = DateTime.Parse(GetExpireDate());
            }
            catch (FormatException)
            {
                ExpireDateResult = "Date format is invalid";
                return;
            }
            
            if(startDate>=endDate)
            {
                ExpireDateResult = "Start date is later than expire date!";
                return;
            }
            IsBusy = true;
            var loginDataToShare = await DataStore.GetItemAsync(ItemId);
            var password = EncService.Decrypt(SecureStorageHelper.GetUserKey().Result, 
                loginDataToShare.Password);
            LoginData newLoginData = new LoginData()
            {   Name = loginDataToShare.Name+" From "+SecureStorageHelper.GetUserEmail(),
                Login = loginDataToShare.Login,
                Password = EncService.Encrypt(App.AppSettings.SecretEncryptionKey, password),
                Website = loginDataToShare.Website,
                Email = loginDataToShare.Email ?? "Not added"    
            };
            var model = new ShareLoginModel
            {   LoginData = newLoginData,
                ReceiverEmail = ReceiverEmail,
                StartDate = startDate,
                EndDate = endDate
            };
            var apiResponse = await _apiService.HandleLoginShare(model);
            IsBusy = false;
            if (!apiResponse.Success)
            {
                MessagingCenter.Send(this, "ShareNotify", apiResponse.Messages.First());
                return;
            }
            MessagingCenter.Send(this, "ShareNotify", apiResponse.Messages.First());
            await App.Current.MainPage.Navigation.PopModalAsync();
            
        }

        private string GetExpireDate()
        {
            var date = ExpireDate + selectedTime;
            var dateS = date.ToString();
            return dateS;
        }


        #region Properties
       

        
        
        
        public string ReceiverEmail
        {
            get => receiverEmail;
            set
            {
                EmailError = "";
                SetProperty(ref receiverEmail, value);
            }
        }
        private string emailError;
        public string EmailError
        {
            get => emailError;
            set => SetProperty(ref emailError, value);
        }
        public DateTime ExpireDate
        {
            get => expireDate;
            set
            {
                ExpireDateResult = "";
                SetProperty(ref expireDate, value);
            }
        }
        public string ExpireDateResult
        {
            get => expireDateResult;
            set => SetProperty(ref expireDateResult, value);
        }
       
        public TimeSpan SelectedTime
        {
            get => selectedTime;
            set => SetProperty(ref selectedTime, value);
        }

        public int ItemId { get; set; }
        #endregion



    }
}
