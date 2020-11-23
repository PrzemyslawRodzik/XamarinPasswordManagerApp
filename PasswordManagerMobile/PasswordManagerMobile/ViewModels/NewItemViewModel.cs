using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;
using PasswordGenerator;
using PasswordManagerApp.Handlers;
using PasswordManagerApp.Services;
using PasswordManagerMobile.Helpers;
using PasswordManagerMobile.Models;

using Xamarin.Forms;

namespace PasswordManagerMobile.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string name;
        private string login;
        private string password;
        private string website;
        private string email;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
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

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string Login
        {
            get => login;
            set => SetProperty(ref login, value);
        }
        private string hibpResult;
        public string HibpResult
        {
            get => hibpResult;
            set => SetProperty(ref hibpResult, value);
        }
        public string Password
        {
            get => password;
            set
            {
                HibpResult = "";
                SetProperty(ref password, value);
            }
        }
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Website
        {
            get => website;
            set => SetProperty(ref website, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command GeneratePasswordCommand { get; }
        public Command HibpCheckCommand { get; }

        private async void OnCancel()
        {
            
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void OnSave()
        {
            LoginData newLoginData = new LoginData()
            {
                Name = Name,
                Login = Login,
                Password = EncService.Encrypt(SecureStorageHelper.GetUserKey().Result, Password),
                Website = Website,
                Email = Email ?? "Not added",
                UserId = SecureStorageHelper.GetUserId().Result
            };

            await DataStore.AddItemAsync(newLoginData);

            // This will pop the current page off the navigation stack
            await App.Current.MainPage.Navigation.PopModalAsync();

        }
        private void OnGeneratePassword()
        {
            Password = new Password(true, true, true, true, 16).Next();
        }

        public void OnHibpCheck()
        {
            IsBusy = true;
            var hibpResult = PwnedPasswords.IsPasswordPwnedAsync(password, new CancellationToken(), null);
            IsBusy = false;
            if (hibpResult <= 0)
                HibpResult = "Your password is OK :)";
            else
                HibpResult = $"Your password have been pwned {hibpResult}. Please, change your password.";

        }
    }
}
