using PasswordManagerMobile.ApiResponses;
using PasswordManagerApp.Handlers;
using PasswordManagerApp.Services;
using PasswordManagerMobile.Configuration;
using PasswordManagerMobile.Helpers;
using PasswordManagerMobile.Models;
using PasswordManagerMobile.Views;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace PasswordManagerMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        
        public Command LoginCommand { get; }
        private ApiService _apiService;
        private readonly JwtHelper _jwtHelper;

        private string email;
        private string password;
        private bool isRunning;


        public LoginViewModel(ApiService apiService)
        {
            LoginCommand = new Command(OnLoginClicked, CanExecute);
            _apiService = apiService;
            _jwtHelper = new JwtHelper();
            this.PropertyChanged +=
                 (_, __) => LoginCommand.ChangeCanExecute();

        }
        private bool CanExecute(object args)
        {
            if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password))
                return false;

            return !IsBusy;
        }

       
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public bool IsRunning
        {
            get => isRunning;
            set => SetProperty(ref isRunning, value);
        }

        

        private async void OnLoginClicked(object obj)
        {
            IsBusy = true;

            bool sessionIsActive = await SecureStorageHelper.CheckIfUserSessionIsActive();
            if (sessionIsActive)
                App.Current.MainPage = new NavigationPage(new AboutPage());
            AuthResponse apiResponse;

           
            // UserDialogs.Instance.ShowLoading("Authenticating");
            
               apiResponse = await _apiService.LogIn(new LoginModel { Email = Email,Password = Password});
            
            


            if (!apiResponse.Success)
            {
                IsBusy = false;
                MessagingCenter.Send(this, "AuthError", apiResponse.Messages.First());
                return;
            }
            if (apiResponse.TwoFactorLogIn)
            {
               await SecureStorage.SetAsync(StorageConstants.UserId, apiResponse.UserId.ToString());
               await SecureStorage.SetAsync(StorageConstants.UserPassword, Password);
               await SecureStorageHelper.SaveUserEmail(Email);
                
                IsBusy = false;
                await App.Current.MainPage.Navigation.PushModalAsync(new TwoFactorPage());
               return;
            }
            ClaimsPrincipal userInfo;
            if (!_jwtHelper.ValidateToken(apiResponse.AccessToken, out userInfo))
            {
                // indicate errors 
                IsBusy = false;
                MessagingCenter.Send(this, "AuthError", "Json web token is invalid");
                return;
            }
            

            await SecureStorageHelper.SaveUserData(apiResponse,userInfo.Identity.Name, Password,Email);
           
            
            App.Current.MainPage = new NavigationPage(new ItemsPage());




        }
        
        
    }
}
