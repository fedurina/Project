using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AppVinil
{
    public class DB
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database = partners;User Id = postgres; Password = postgres;");
        public void openConnection()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
        }
        public void closeConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
        public NpgsqlConnection GetConnection()
        {
            return conn;
        }
        public int DBLenth()
        {
            DB db = new DB();
            db.openConnection();


            NpgsqlCommand command = new NpgsqlCommand("Select Vinil_image FROM Vinil ", db.GetConnection());

            var reader = command.ExecuteReader();
            List<string> npgStrings = new List<string>();
            int k = 0;

            // Проверка наличия строк
            while (reader.Read())
            {
                k += 1;
            }
            db.closeConnection();
            return k;
        }
        public List<string> VinilListItems(string str)
        {
            DB db = new DB();
            db.openConnection();


            NpgsqlCommand command = new NpgsqlCommand(str, db.GetConnection());

            var reader = command.ExecuteReader();
            List<string> npgStrings = new List<string>();

            // Проверка наличия строк
            while (reader.Read())
            {
                // Получение значения строки
                string npgString = reader.GetString(0);

                // Добавление значения в список
                npgStrings.Add(npgString);
            }
            db.closeConnection();
            return npgStrings;
        }

        public List<Vinil> GetItemsVinil(string str)
        {
            DB db = new DB();
            db.openConnection();


            NpgsqlCommand command = new NpgsqlCommand(str, db.GetConnection());

            var reader = command.ExecuteReader();
            List<Vinil> npgStrings = new List<Vinil>();

            // Проверка наличия строк
            while (reader.Read())
            {
                Vinil vinil = new Vinil();


                vinil.Id = reader.GetInt32(0);
                vinil.NameVinil = reader.GetString(1);
                vinil.NameSinger = reader.GetString(2);
                int temp = reader.GetInt32(3);
                vinil.StudioVinil = GetStringID($"Select studio_name FROM studio WHERE studio_id ='{temp}'");
                vinil.Country = GetStringID($"Select country_name FROM country JOIN studio ON country.country_id = studio.country_id WHERE studio_id = '{temp}'");
                vinil.Year = reader.GetString(4);
                vinil.Genre = GetStringID($"Select genre_name FROM genre WHERE genre_id = '{reader.GetInt32(5)}'");
                vinil.User = reader.GetInt32(6);
                vinil.Price = reader.GetInt32(7);
                vinil.Image = reader.GetString(8);
                vinil.Booking = reader.GetBoolean(9);
                vinil.Condition = GetStringID($"Select conditions_name FROM conditions WHERE conditions_id = '{reader.GetInt32(10)}'");


                npgStrings.Add(vinil);
            }
            db.closeConnection();
            return npgStrings;
        }
        public string GetStringID(string str)
        {
            DB db = new DB();
            db.openConnection();

            NpgsqlCommand command = new NpgsqlCommand(str, db.GetConnection());

            var reader = command.ExecuteReader();
            string res = "";
            while (reader.Read())
            {


                res = reader.GetString(0);
                
            }
            db.closeConnection();
            return res;
        }
        public int GetIntID(string str)
        {
            DB db = new DB();
            db.openConnection();

            NpgsqlCommand command = new NpgsqlCommand(str, db.GetConnection());

            var reader = command.ExecuteReader();
            int res = 0;
            while (reader.Read())
            {


                res = reader.GetInt32(0);

            }
            db.closeConnection();
            return res;
        }

    }
}
