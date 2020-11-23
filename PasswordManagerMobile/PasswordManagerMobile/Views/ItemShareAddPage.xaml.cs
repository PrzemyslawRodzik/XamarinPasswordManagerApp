using System.ComponentModel;
using Xamarin.Forms;
using PasswordManagerMobile.ViewModels;
using PasswordManagerMobile.Models;

namespace PasswordManagerMobile.Views
{
    public partial class ItemShareAddPage : ContentPage
    {

        public ItemShareAddPage(int id)
        {

            InitializeComponent(); 
            MessagingCenter.Subscribe<ItemShareAddViewModel, string>(this, "ShareNotify", (sender, message) =>
            {
                DisplayAlert("ShareNotify", message, "Ok");
            });
            BindingContext = new ItemShareAddViewModel(id);





            
        }
    }
}