using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PasswordManagerMobile.Models;
using PasswordManagerMobile.Views;
using PasswordManagerMobile.ViewModels;

namespace PasswordManagerMobile.Views
{
    public partial class SharedItemsPage : ContentPage
    {
        ItemShareViewModel _viewModel;

        public SharedItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemShareViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        
    }
}