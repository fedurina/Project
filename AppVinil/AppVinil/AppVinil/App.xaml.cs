using AppVinil.Services;
using AppVinil.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppVinil
{
    public partial class App : Application
    {
        public static DB Db;
        
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
