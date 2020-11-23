using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordManagerApp.Services;
using PasswordManagerMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasswordManagerMobile.Views
{
    
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<LoginViewModel,string>(this, "AuthError",(sender,message)=>
            {
                DisplayAlert("AuthError", message, "Ok");
            });
            this.BindingContext = new LoginViewModel(new ApiService());
        }
         

    }
}