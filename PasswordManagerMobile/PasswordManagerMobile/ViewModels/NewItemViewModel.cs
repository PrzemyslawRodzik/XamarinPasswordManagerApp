using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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

        public string Website
        {
            get => website;
            set => SetProperty(ref website, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

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
    }
}
