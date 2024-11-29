using AppVinil.Models;
using AppVinil.ViewModels;
using AppVinil.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppVinil.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;
        private static DB basedataItem = new DB();
        public static List<Vinil> myVinilesList = basedataItem.GetItemsVinil($"SELECT * FROM vinil WHERE user_id ='{basedataItem.GetIntID($"Select user_id FROM users WHERE user_mail = '{LoginPage.LogInMail}'")}'");

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();

        }
        private void ShowItemsVinil()
        {

            myVinilCollection.ItemsSource = myVinilesList;
        }

        protected async override void OnAppearing()
        {
            if (LoginPage.LogInMail == "No")
            {
                await DisplayAlert("Error", "Вы не авторизованы", "OK");

                return;
            }
            base.OnAppearing();
            _viewModel.OnAppearing();
            ShowItemsVinil();
        }
        private async void BookingsButton(object sender, EventArgs e)
        {
            if (LoginPage.LogInMail == "No")
            {
                await DisplayAlert("Error", "Вы не авторизованы", "OK");

                return;
            }
            DB db = new DB();


            myVinilesList = db.GetItemsVinil($"Select * FROM vinil JOIN booking ON vinil.vinil_id = booking.vinil_id WHERE booking.user_id = '{db.GetIntID($"Select user_id FROM users WHERE user_mail = '{LoginPage.LogInMail}'")}'");

            ShowItemsVinil();


        }
        private async void MyVinilItemsButton(object sender, EventArgs e)
        {
            if (LoginPage.LogInMail == "No")
            {
                await DisplayAlert("Error", "Вы не авторизованы", "OK");

                return;
            }
            DB db = new DB();


            myVinilesList = db.GetItemsVinil($"SELECT * FROM vinil WHERE user_id ='{db.GetIntID($"Select user_id FROM users WHERE user_mail = '{LoginPage.LogInMail}'")}'");

            ShowItemsVinil();


        }
    }
}