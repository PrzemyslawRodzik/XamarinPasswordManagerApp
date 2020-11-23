using System.ComponentModel;
using Xamarin.Forms;
using PasswordManagerMobile.ViewModels;
using PasswordManagerMobile.Models;

namespace PasswordManagerMobile.Views
{
    public partial class ItemShareDetailPage : ContentPage
    {
        ItemShareDetailViewModel _viewModel;

        public ItemShareDetailPage(SharedLoginModel item)
        {
            
            InitializeComponent();
            MessagingCenter.Subscribe<ItemShareDetailViewModel, string>(this, "ShareDetailNotify", (sender, message) =>
            {
                DisplayAlert("", message, "Ok");
            });
            BindingContext = _viewModel = new ItemShareDetailViewModel(item);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}