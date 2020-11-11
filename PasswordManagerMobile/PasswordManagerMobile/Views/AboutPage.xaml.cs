using PasswordManagerApp.Services;
using PasswordManagerMobile.Helpers;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasswordManagerMobile.Views
{
    public partial class AboutPage : ContentPage
    {
        
        
        public AboutPage()
        {
            InitializeComponent();
        }
        
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            SecureStorageHelper.ClearData();
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }
        
    }
}