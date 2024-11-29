using AppVinil.ViewModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppVinil.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static string LogInMail = "No";
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
        private async void LogInButton(object sender, EventArgs e)
        {
            if ((loginImail.Text == null))
            {
                await DisplayAlert("Error", "Введите почту в поле ввода", "OK");
                return;
            }
            else if ((loginiPassword.Text == null))
            {
                await DisplayAlert("Error", "Введите пароль в поле ввода", "OK");
                return;
            }
            string title1 = loginImail.Text.Trim();
            string title2 = loginiPassword.Text.Trim();
            DB db = new DB();
            if (!db.VinilListItems("Select user_mail FROM users").Contains(title1))
            {
                await DisplayAlert("Error", "Вы не зарегистрированы", "OK");
                loginImail.Text = "";
                loginiPassword.Text = "";
                return;
            }
            else if (db.GetStringID($"Select user_password FROM users WHERE user_mail = '{title1}'")!= title2)
            {
                await DisplayAlert("Error", "Введен неверный пароль", "OK");
                loginiPassword.Text = "";
                return;
            }
            LogInMail = title1;
            string LogInName = db.GetStringID($"Select user_name FROM users WHERE user_mail = '{title1}'");
            ItemsPage.myVinilesList = db.GetItemsVinil($"SELECT * FROM vinil WHERE user_id ='{db.GetIntID($"Select user_id FROM users WHERE user_mail = '{LogInMail}'")}'");

            await DisplayAlert("Вход", $"Добро пожаловать {LogInName}!", "OK");
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");


        }
        private async void RegistrationButton(object sender, EventArgs e)
        {
            if ((registrationImail.Text == null))
            {
                await DisplayAlert("Error", "Введите почту в поле ввода", "OK");
                return;
            }
            else if ((registrationName.Text == null))
            {
                await DisplayAlert("Error", "Введите имя пользователя в поле ввода", "OK");
                return;
            }
            else if ((registrationPassword.Text == null))
            {
                await DisplayAlert("Error", "Введите пароль в поле ввода", "OK");
                return;
            }
            string title1 = registrationImail.Text.Trim();
            string title2 = registrationName.Text.Trim();
            string title3 = registrationPassword.Text.Trim();
            DB db = new DB();
            if (db.VinilListItems("Select user_mail FROM users").Contains(title1))
            {
                await DisplayAlert("Error", "Вы уже зарегистрированы", "OK");
                registrationImail.Text = "";
                registrationName.Text = "";
                registrationPassword.Text = "";
                return;
            }
            db.openConnection();

            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO users(user_id, user_name, user_password, user_mail) VALUES((select user_id + 1 from users where user_id = (select max(user_id) from users)), @title2, @title3, @title1)", db.GetConnection());
           
            command.Parameters.AddWithValue("@title2", title2);
            command.Parameters.AddWithValue("@title3", title3);
            command.Parameters.AddWithValue("@title1", title1);
            NpgsqlDataReader dr = command.ExecuteReader();
            db.closeConnection();

            string LogInName = db.GetStringID($"Select user_name FROM users WHERE user_mail = '{title1}'");


            await DisplayAlert($"Вы зарегистрировались {LogInName}","!Для того что бы войти в ситему войдите в аккаунт", "OK");
            registrationImail.Text = "";
            registrationName.Text = "";
            registrationPassword.Text = "";


        }
    }
}