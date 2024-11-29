using AppVinil.Models;
using AppVinil.ViewModels;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppVinil.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }


        public NewItemPage()
        {
            InitializeComponent();

            BindingContext = new NewItemViewModel();
        }
        private async void AddNewButton(object sender, EventArgs e)
        {
            if (LoginPage.LogInMail == "No")
            {
                await DisplayAlert("Error", "Вы не авторизованы", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }
            if ((addName.Text == null))
            {
                await DisplayAlert("Error", "Введите название пластинки в поле ввода", "OK");
                return;
            }
            else if ((addSinger.Text == null))
            {
                await DisplayAlert("Error", "Введите имя исполнителя в поле ввода", "OK");
                return;
            }
            else if ((addStudio.Text == null))
            {
                await DisplayAlert("Error", "Введите имя студии в поле ввода", "OK");
                return;
            }
            else if ((addYear.Text == null))
            {
                await DisplayAlert("Error", "Введите год выпуска пластинки в поле ввода", "OK");
                return;
            }
            else if ((addGenre.Text == null))
            {
                await DisplayAlert("Error", "Введите жанр в поле ввода", "OK");
                return;
            }
            else if ((addPrice.Text == null))
            {
                await DisplayAlert("Error", "Введите стоимость пластинки в поле ввода", "OK");
                return;
            }
            else if ((addURL.Text == null))
            {
                await DisplayAlert("Error", "Введите ссылку на обложку альбома в поле ввода", "OK");
                return;
            }
            else if ((addCondition.Text == null))
            {
                await DisplayAlert("Error", "Введите состояние пластинки в поле ввода", "OK");
                return;
            }


            DB db = new DB();

            if (!(db.VinilListItems("Select studio_name FROM studio").Contains(addStudio.Text.Trim())))
            {
                await DisplayAlert("Error", "Такой студии не существует обратитетсь в поддержку", "OK");
                addStudio.Text = "";
                return;
            }
            else if (!(db.VinilListItems("Select genre_name FROM genre").Contains(addGenre.Text.Trim())))
            {
                await DisplayAlert("Error", "Такого жанра не существует", "OK");
                addGenre.Text = "";
                return;
            }
            else if (!(db.VinilListItems("Select conditions_name FROM conditions").Contains(addCondition.Text.Trim())))
            {
                await DisplayAlert("Error", "Такого вида состояния винила не существует", "OK");
                addCondition.Text = "";
                return;
            }

            string title1 = addName.Text.Trim();
            string title2 = addSinger.Text.Trim();
            int title3 = db.GetIntID($"Select studio_id FROM studio WHERE studio_name = '{addStudio.Text.Trim()}'");
            string title4 = addYear.Text.Trim();
            int title5 = db.GetIntID($"Select genre_id FROM genre WHERE genre_name = '{addGenre.Text.Trim()}'");
            string title = addPrice.Text.Trim();
            int title6 = int.Parse(title);
            string title7 = addURL.Text.Trim();
            int title8 = db.GetIntID($"Select conditions_id FROM conditions WHERE conditions_name = '{addCondition.Text.Trim()}'");
            int titlelUser = db.GetIntID($"Select user_id FROM users WHERE user_mail = '{LoginPage.LogInMail}'");
            bool titleBook = true;
            db.openConnection();

            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO vinil(vinil_id, vinil_name, vinil_singer, studio_id, vinil_year, genre_id, user_id, vinil_price, vinil_image, vinil_booking, condition_id) VALUES((select vinil_id + 1 from vinil where vinil_id = (select max(vinil_id) from vinil)), @title1, @title2, @title3, @title4, @title5, @titlelUser, @title6, @title7, @titleBook, @title8)", db.GetConnection());

            command.Parameters.AddWithValue("@title1", title1);
            command.Parameters.AddWithValue("@title2", title2);
            command.Parameters.AddWithValue("@title3", title3);
            command.Parameters.AddWithValue("@title4", title4);
            command.Parameters.AddWithValue("@title5", title5);
            command.Parameters.AddWithValue("@titlelUser", titlelUser);
            command.Parameters.AddWithValue("@title6", title6);
            command.Parameters.AddWithValue("@title7", title7);
            command.Parameters.AddWithValue("@titleBook", titleBook);
            command.Parameters.AddWithValue("@title8", title8);
            NpgsqlDataReader dr = command.ExecuteReader();
            db.closeConnection();

            AboutPage.vinilesList = db.GetItemsVinil("SELECT * FROM vinil");

            await DisplayAlert("Great","Ваша пластинка была успешно добавлена", "OK");
            await Shell.Current.GoToAsync("..");


        }
    }
}