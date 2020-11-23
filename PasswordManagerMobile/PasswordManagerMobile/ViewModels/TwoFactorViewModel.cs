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

namespace PasswordManagerMobile.ViewModels
{
    public class TwoFactorViewModel : BaseViewModel
    {
        public Command SendTokenCommand { get; }
        private ApiService _apiService;
        private readonly JwtHelper _jwtHelper;
        public string Token { get; set; }
        

        public TwoFactorViewModel(ApiService apiService)
        {
            SendTokenCommand = new Command(OnSendTokenClicked);
            _apiService = apiService;
            _jwtHelper = new JwtHelper();

        }

        


        private async void OnSendTokenClicked(object obj)
        {
            if (SecureStorageHelper.CheckIfUserSessionIsActive().Result)
                App.Current.MainPage = new NavigationPage(new ItemsPage());
            if (string.IsNullOrEmpty(Token))
            {
                MessagingCenter.Send(this, "AuthError", "Token cannot be empty.");
                return;
            }


            IsBusy = true;
            
            ApiTwoFactorResponse apiResponse = await _apiService.TwoFactorLogIn(await SecureStorageHelper.GetUserId(), Token);
            IsBusy = false;

            if (apiResponse.VerificationStatus != 1)
            {
                if (apiResponse.VerificationStatus == 0)
                {
                    MessagingCenter.Send(this, "AuthError", apiResponse.Messages.First());
                    return;

                }
                else
                {
                    MessagingCenter.Send(this, "AuthError", "Your code has expired. Try log in again.");
                    SecureStorageHelper.ClearData();
                    await App.Current.MainPage.Navigation.PopModalAsync();
                    return;
                }
            }

            IsBusy = true;
            if (!_jwtHelper.ValidateToken(apiResponse.AccessToken, out _))
            {
                // indicate errors 
                MessagingCenter.Send(this, "AuthError", "Json web token is invalid");
                await App.Current.MainPage.Navigation.PopModalAsync();
                return;
            }


            await SecureStorageHelper.SaveUserData(apiResponse);



            //App.Current.MainPage.Navigation.InsertPageBefore(new ItemsPage(), TwoFactorPage);
           // await Navigation.PopAsync();

            await App.Current.MainPage.Navigation.PopModalAsync();
            IsBusy = false;
            App.Current.MainPage = new NavigationPage(new ItemsPage());



        }
        
        

    }
}
