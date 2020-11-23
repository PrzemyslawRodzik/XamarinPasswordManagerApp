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
    public class ItemShareViewModel : BaseViewModel
    {
        private SharedLoginModel _selectedItem;
        

        public ObservableCollection<SharedLoginModel> Items { get; }
        
        public Command<SharedLoginModel> ItemTapped { get; }
        public Command<SharedLoginModel> ItemSwiped { get; }
        public Command LoadSharedItemsCommand { get; }

        

        public ItemShareViewModel()
        {
            

            Title = "Shared Logins";
            Items = new ObservableCollection<SharedLoginModel>();
            LoadSharedItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<SharedLoginModel>(OnItemSelected);
            this.PropertyChanged +=
                 (_, __) => ItemTapped.ChangeCanExecute();
            ItemSwiped = new Command<SharedLoginModel>(OnItemSwiped);
            this.PropertyChanged +=
                 (_, __) => ItemSwiped.ChangeCanExecute();
            

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
                var items =  await DataStore.GetSharedItems(true);
                
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
            SelectedItem = null;
             
        }
        public void OnDisappearing()
        {
           // IsBusy = true;
           // SelectedItem = null;
        }

        public SharedLoginModel SelectedItem
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
        

        async void OnItemSelected(SharedLoginModel item)
        {   if (IsBusy)
                return;
            
            IsBusy = true;
            if (item == null)
                return;

            IsBusy = false;
            await App.Current.MainPage.Navigation.PushModalAsync(new ItemShareDetailPage(item));
        }
        async void OnItemSwiped(SharedLoginModel item)
        {
           await App.Current.MainPage.Navigation.PushModalAsync(new ItemShareDetailPage(item));
        }
       



    }
}