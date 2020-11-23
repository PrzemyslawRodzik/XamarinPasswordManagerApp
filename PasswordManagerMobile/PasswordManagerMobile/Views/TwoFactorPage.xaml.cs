using PasswordManagerApp.Services;
using PasswordManagerMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasswordManagerMobile.Views
{
    
    public partial class TwoFactorPage : ContentPage
    {
        public TwoFactorPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<TwoFactorViewModel, string>(this, "AuthError", (sender, message) =>
            {
                DisplayAlert("AuthError", message, "Ok");
            });
            this.BindingContext = new TwoFactorViewModel(new ApiService());
        }
    }
}