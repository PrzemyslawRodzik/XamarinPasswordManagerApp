using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PasswordGenerator;
using PasswordManagerApp.Handlers;
using PasswordManagerMobile.Helpers;
using PasswordManagerMobile.Models;
using Xamarin.Forms;

namespace PasswordManagerMobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemUpdateViewModel : BaseViewModel
    {
        
        private int itemId;
        private string name;
        private string login;
        private string password;
        private string website;
        private string email;
        
        
        public string Id { get; set; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command GeneratePasswordCommand { get; }
        public Command HibpCheckCommand { get; }

        public ItemUpdateViewModel(int itemId)
        {
            ItemId = itemId;
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                 (_, __) => SaveCommand.ChangeCanExecute();
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                 (_, __) => CancelCommand.ChangeCanExecute();
            GeneratePasswordCommand = new Command(OnGeneratePassword);
            HibpCheckCommand = new Command(OnHibpCheck);



            
        }

        

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name)
                && !String.IsNullOrWhiteSpace(login)
                && !String.IsNullOrWhiteSpace(password)
                && !String.IsNullOrWhiteSpace(website);



        }
        private void OnCancel()
        {
             App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void OnSave()
        {
            LoginData newLoginData = new LoginData()
            {   Id = Int32.Parse(Id),
                Name = Name,
                Login = Login,
                Password = EncService.Encrypt(SecureStorageHelper.GetUserKey().Result, Password),
                Website = Website,
                Email = Email ?? "Not added",
                UserId = SecureStorageHelper.GetUserId().Result
            };

            await DataStore.UpdateItemAsync(newLoginData);

            // This will pop the current page off the navigation stack
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private bool CanExecute(object args)
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

        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }
        #endregion
        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id.ToString();
                Name = item.Name;
                Login = item.Login;
                Password = EncService.Decrypt(SecureStorageHelper.GetUserKey().Result,item.Password);
                Website = item.Website;
                Email = item.Email;
                
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        private void OnGeneratePassword()
        {
            Password =  new Password(true, true, true, true, 16).Next();
        }
        
        public void OnHibpCheck()
        {
            
            


            var hibpResult = PwnedPasswords.IsPasswordPwnedAsync(password, new CancellationToken(), null);
            
            if (hibpResult <= 0)
                Password =  "Your password is OK :)";
            else
                Password =  $"Your password have been pwned {hibpResult}. Please, change your password.";
           
        }

    }
}
