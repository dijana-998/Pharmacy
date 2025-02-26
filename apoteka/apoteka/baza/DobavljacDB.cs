using apoteka.model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;

namespace apoteka.baza
{
    class DobavljacDB
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ApotekaDBConnection"].ConnectionString;

        public static List<Dobavljac> GetAllDobavljaci()
        {
            List<Dobavljac> dobavljaciList = new List<Dobavljac>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("select iddobavljac, naziv, telefon, adresa, email from dobavljac", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Dobavljac dobavljaci = new Dobavljac
                    {
                        iddobavljac = reader.GetInt32(0),
                        naziv = reader.IsDBNull(1) ? null : reader.GetString(1),
                        telefon = reader.IsDBNull(2) ? null : reader.GetString(2),
                        adresa = reader.IsDBNull(3) ? null : reader.GetString(3),
                        email = reader.IsDBNull(4) ? null : reader.GetString(4),
                    };
                    dobavljaciList.Add(dobavljaci);
                }
            }

            return dobavljaciList;
        }

    }
}
