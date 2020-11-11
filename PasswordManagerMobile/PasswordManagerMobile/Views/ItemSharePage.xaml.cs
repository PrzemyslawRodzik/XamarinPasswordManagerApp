using System.ComponentModel;
using Xamarin.Forms;
using PasswordManagerMobile.ViewModels;
using PasswordManagerMobile.Models;

namespace PasswordManagerMobile.Views
{
    public partial class ItemSharePage : ContentPage
    {

        public ItemSharePage(int id)
        {

            InitializeComponent(); 
            MessagingCenter.Subscribe<ItemShareViewModel, string>(this, "ShareNotify", (sender, message) =>
            {
                DisplayAlert("ShareNotify", message, "Ok");
            });
            BindingContext = new ItemShareViewModel(id);





            
        }
    }
}