using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using PasswordManagerMobile.Models;
using PasswordManagerMobile.Views;
using PasswordManagerMobile.Helpers;

namespace PasswordManagerMobile.ViewModels
{
    public class LoginDataViewModel : BaseViewModel
    {
        private LoginData _selectedItem;
        

        public ObservableCollection<LoginData> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command LogoutCommand { get; }
        
        public Command<LoginData> ItemTapped { get; }
        public Command<LoginData> ItemSwiped { get; }
        public Command LoadSharedItemsCommand { get; }

        

        public LoginDataViewModel()
        {
            

            Title = "Logins";
            Items = new ObservableCollection<LoginData>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<LoginData>(OnItemSelected);
            this.PropertyChanged +=
                 (_, __) => ItemTapped.ChangeCanExecute();
            ItemSwiped = new Command<LoginData>(OnItemSwiped);
            this.PropertyChanged +=
                 (_, __) => ItemSwiped.ChangeCanExecute();

            AddItemCommand = new Command(OnAddItem,CanExecute);
            this.PropertyChanged +=
                 (_, __) => AddItemCommand.ChangeCanExecute();
            LoadSharedItemsCommand = new Command(OnLoadSharedItems, CanExecute);
            this.PropertyChanged +=
                 (_, __) => LoadSharedItemsCommand.ChangeCanExecute();
            LogoutCommand = new Command(OnLogout);

        }
        private bool CanExecute(object obj)
        {
            return !IsBusy;
        }
        

        async Task ExecuteLoadItemsCommand()
        {   
            IsBusy = true;
            if (!SecureStorageHelper.CheckIfUserSessionIsActive().Result)
                App.Current.MainPage = new NavigationPage(new LoginPage());

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            ExecuteLoadItemsCommand();
            SelectedItem = null;
             
        }
        public void OnDisappearing()
        {
           // IsBusy = true;
           // SelectedItem = null;
        }

        public LoginData SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }
        
        
         
        private  void OnAddItem(object obj)
        {
            
            IsBusy = true;
            
            App.Current.MainPage.Navigation.PushModalAsync(new NewItemPage());
            IsBusy = false;
               
        }
        private void OnLogout(object obj)
        {

            IsBusy = true;
            SecureStorageHelper.ClearData();
            App.Current.MainPage = new NavigationPage(new LoginPage());
            IsBusy = false;

        }


        async void OnItemSelected(LoginData item)
        {   if (IsBusy)
                return;
            
            IsBusy = true;
            if (item == null)
                return;

            IsBusy = false;
            await App.Current.MainPage.Navigation.PushModalAsync(new ItemDetailPage(item.Id));
        }
        async void OnItemSwiped(LoginData item)
        {
            
            await App.Current.MainPage.Navigation.PushModalAsync(new ItemDetailPage(item.Id));
        }
        async void OnLoadSharedItems(object obj)
        {
            IsBusy = true;
            await App.Current.MainPage.Navigation.PushAsync(new SharedItemsPage());
            IsBusy = false;
        }



    }
}