using AppVinil.ViewModels;
using AppVinil.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppVinil
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            //if (1==1)
            //{
            //    await DisplayAlert("Error", "Пластинок с таким жанром сейчас нет в наличии", "OK");
            //
            //    return;
            //}
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
