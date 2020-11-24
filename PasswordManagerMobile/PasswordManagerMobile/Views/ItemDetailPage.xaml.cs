using System.ComponentModel;
using Xamarin.Forms;
using PasswordManagerMobile.ViewModels;
using PasswordManagerMobile.Models;

namespace PasswordManagerMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel _viewModel;
        

        public ItemDetailPage(int id)
        {
            
            InitializeComponent();
            MessagingCenter.Subscribe<ItemDetailViewModel, string>(this, "DetailNotify",(sender, message) =>
            {
                DisplayAlert("",message, "Ok");
            });
            BindingContext = _viewModel = new ItemDetailViewModel(id);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}