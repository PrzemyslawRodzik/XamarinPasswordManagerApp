using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PasswordManagerApp.Handlers;
using PasswordManagerMobile.Helpers;
using PasswordManagerMobile.Models;
using PasswordManagerMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PasswordManagerMobile.ViewModels
{
    
    public class ItemShareDetailViewModel : BaseViewModel
    {
        
        private SharedLoginModel sharedItem;
        public Command HibpCheckCommand { get; set; }
        public Command CopyCommand { get; set; }

        private string name;
        private string login;
        private string password;
        private string website;
        private string email;
        private string expireDate;
        
        
        public string Id { get; set; }
        

        public ItemShareDetailViewModel(SharedLoginModel sharedItem)
        {
            SharedItem = sharedItem;
            CopyCommand = new Command(OnCopy);
            HibpCheckCommand = new Command(OnHibpCheck);




        }

        public void OnAppearing()
        {
            IsBusy = true;
            LoadSharedItem(SharedItem);
            IsBusy = false;
        }
        private bool CanExecute()
        {
            return !IsBusy;
        }
        #region Properties
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Website
        {
            get => website;
            set => SetProperty(ref website, value);
        }
        public string Login
        {
            get => login;
            set => SetProperty(ref login, value);
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        public string ExpireDate
        {
            get => expireDate;
            set => SetProperty(ref expireDate, value);
        }

        public SharedLoginModel SharedItem
        {
            get
            {
                return sharedItem;
            }
            set
            {
                sharedItem = value;
                LoadSharedItem(value);
            }
        }
        #endregion
        public  void LoadSharedItem(SharedLoginModel item)
        {
            try
            {
                
                Id = item.LoginData.Id.ToString();
                Name = item.LoginData.Name;
                Login = item.LoginData.Login;
                Password = EncService.Decrypt(App.AppSettings.SecretEncryptionKey, item.LoginData.Password);
                Website = item.LoginData.Website;
                Email = item.LoginData.Email;
                ExpireDate = item.EndDate.ToString();
                
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        private void OnCopy()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Clipboard.SetTextAsync(Password);
                MessagingCenter.Send(this, "ShareDetailNotify", "Your password has been copied to clipboard.");

            });
        }

        private void OnHibpCheck()
        {
            IsBusy = true;
            var hibpResult = PwnedPasswords.IsPasswordPwnedAsync(password, new CancellationToken(), null);
            IsBusy = false;
            if (hibpResult <= 0)
                MessagingCenter.Send(this, "ShareDetailNotify", "Your password is Ok :)");
            else
                MessagingCenter.Send(this, "ShareDetailNotify", $"Your password have been pwned {hibpResult}. Please, change your password.");


        }







    }
}
