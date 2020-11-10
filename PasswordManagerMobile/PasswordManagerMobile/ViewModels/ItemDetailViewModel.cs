using System;
using System.Diagnostics;
using System.Threading.Tasks;
using PasswordManagerMobile.Helpers;
using PasswordManagerMobile.Models;
using PasswordManagerMobile.Views;
using Xamarin.Forms;

namespace PasswordManagerMobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        
        private int itemId;
        private string name;
        private string login;
        private string password;
        private string website;
        private string email;
        
        
        public string Id { get; set; }
        public Command UpdateCommand { get; }

        public ItemDetailViewModel(int itemId)
        {
            ItemId = itemId;
            UpdateCommand = new Command(OnUpdateItem, CanExecute);
            this.PropertyChanged +=
                 (_, __) => UpdateCommand.ChangeCanExecute();
        }

        public void OnAppearing()
        {
            IsBusy = true;
            LoadItemId(ItemId);
            IsBusy = false;
        }
        private bool CanExecute()
        {
            return !IsBusy;
        }
        #region Properties
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Website
        {
            get => website;
            set => SetProperty(ref website, value);
        }
        public string Login
        {
            get => login;
            set => SetProperty(ref login, value);
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }
        #endregion
        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id.ToString();
                Name = item.Name;
                Login = item.Login;
                Password = EncService.Decrypt(SecureStorageHelper.GetUserKey().Result,item.Password);
                Website = item.Website;
                Email = item.Email;
                
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        private async void OnUpdateItem()
        {
            IsBusy = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new ItemUpdatePage(ItemId));
            IsBusy = false;
        }
    }
}
