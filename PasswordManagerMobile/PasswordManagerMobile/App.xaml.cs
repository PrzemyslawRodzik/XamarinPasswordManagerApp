using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PasswordManagerMobile.Services;
using PasswordManagerMobile.Views;
using PasswordManagerMobile.Models;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

using PasswordManagerMobile.Helpers;
using PasswordManagerApp.Services;

namespace PasswordManagerMobile
{
    public partial class App : Application
    {
        

        public App()
        {
            InitializeComponent();

            DependencyService.Register<WebDataStore>();
            DependencyService.Register<EncryptionService>();
            
            if (SecureStorageHelper.CheckIfUserSessionIsActive().Result)
                MainPage = new NavigationPage(new ItemsPage());
            else
                MainPage = new NavigationPage(new LoginPage());
           
        }

        private static AppSettings appSettings;
        public static AppSettings AppSettings
        {
            get
            {
                if (appSettings == null)
                    LoadAppSettings();

                return appSettings;
            }
        }
        private static void LoadAppSettings()
        { 
            var appSettingsResourceStream = Assembly.GetAssembly(typeof(AppSettings)).GetManifestResourceStream("PasswordManagerMobile.Configuration.appsettings.json");

            if (appSettingsResourceStream == null)
                return;

            using (var streamReader = new StreamReader(appSettingsResourceStream))
            {
                var jsonString = streamReader.ReadToEnd();
                appSettings = JsonConvert.DeserializeObject<AppSettings>(jsonString);
            }
        }

        protected override void OnStart()
        {
            //SecureStorageHelper.ClearData();
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
            

        }
        
    }
}
