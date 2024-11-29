using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppVinil.ViewModels;
using Npgsql;

namespace AppVinil.Views
{
    public partial class AboutPage : ContentPage
    {
        private static DB basedata = new DB();
        public static List<Vinil> vinilesList= basedata.GetItemsVinil("SELECT * FROM vinil WHERE vinil_booking = true");

        public AboutPage()
        {

            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ShowItemsVinil();
        }
        private void ShowItemsVinil()
        {

            vinilCollection.ItemsSource = vinilesList;
            
        }

        private async void SearchVinilButton(object sender, EventArgs e)
        {
            if ((nameField.Text == null))
            {
                await DisplayAlert("Error", "Введите название в поле ввода", "OK");
                return;
            }
            string title = nameField.Text.Trim();
            DB db = new DB();
            if ((title.Length < 1))
            {
                await DisplayAlert("Error", "Введите название в поле ввода", "OK");
                return;
            }
            else if (!db.VinilListItems("Select vinil_name FROM vinil WHERE vinil_booking = true").Contains(title))
            {
                await DisplayAlert("Error", "Пластинок с таким названием сейчас нет в наличии", "OK");
                nameField.Text = "";
                return;
            }
            
            vinilesList = db.GetItemsVinil($"SELECT * FROM vinil  WHERE vinil_booking = true and vinil_name = '{title}'");
            
            ShowItemsVinil();
            

        }
        private async void OnGenreRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            DB db = new DB();
            RadioButton radioButton = (RadioButton) sender;
            string g = radioButton.Value.ToString();
            if (g == "Все")
            {
                vinilesList = db.GetItemsVinil("SELECT * FROM vinil WHERE vinil_booking = true");
                ShowItemsVinil();
                
                return;
            }
            else if (!db.VinilListItems("Select genre_name FROM genre").Contains(g))
            {
                await DisplayAlert("Error", "Пластинок с таким жанром сейчас нет в наличии", "OK");
                
                return;
            }

            vinilesList = db.GetItemsVinil($"SELECT * FROM vinil JOIN genre ON vinil.genre_id = genre.genre_id WHERE genre_name = '{g}' and vinil_booking = true");

            ShowItemsVinil();
        }
        private async void BronirovanieButton(object sender, EventArgs e)
        {
            if (LoginPage.LogInMail == "No")
            {
                await DisplayAlert("Error", "Вы не авторизованы", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }
            DB db = new DB();
            Button button = (Button)sender;
            
            string title = button.Text.ToString();
            int id = int.Parse(title);
            db.openConnection();

            NpgsqlCommand command = new NpgsqlCommand("UPDATE vinil SET vinil_booking = false WHERE vinil_id= @id ", db.GetConnection());

            command.Parameters.AddWithValue("@id", id);
            NpgsqlDataReader dr = command.ExecuteReader();
            db.closeConnection();
            vinilesList = db.GetItemsVinil($"SELECT * FROM vinil  WHERE vinil_booking = true");
            ShowItemsVinil();
            await DisplayAlert("Great", $"Вы успешно забронировали пластинку №: {id} почта владельца: {db.GetStringID($"SELECT user_mail FROM vinil JOIN users ON vinil.user_id = users.user_id WHERE vinil_id = '{id}' ")} ", "OK");


        }
    }
}