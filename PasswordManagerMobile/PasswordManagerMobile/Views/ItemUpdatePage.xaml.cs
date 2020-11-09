using System.ComponentModel;
using Xamarin.Forms;
using PasswordManagerMobile.ViewModels;
using PasswordManagerMobile.Models;

namespace PasswordManagerMobile.Views
{
    public partial class ItemUpdatePage : ContentPage
    {

        public ItemUpdatePage(int id)
        {

            InitializeComponent();
            BindingContext = new ItemUpdateViewModel(id);
        }
    }
}