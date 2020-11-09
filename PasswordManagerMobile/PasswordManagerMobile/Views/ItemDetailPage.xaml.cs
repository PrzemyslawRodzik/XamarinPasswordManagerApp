using System.ComponentModel;
using Xamarin.Forms;
using PasswordManagerMobile.ViewModels;
using PasswordManagerMobile.Models;

namespace PasswordManagerMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        
        public ItemDetailPage(int id)
        {
            
            InitializeComponent();
            BindingContext = new ItemDetailViewModel(id);
        }
    }
}