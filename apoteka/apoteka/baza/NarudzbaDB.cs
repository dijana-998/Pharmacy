using apoteka.model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace apoteka.baza
{
    internal class NarudzbaDB
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ApotekaDBConnection"].ConnectionString;

        public static int DodajNarudzbu(Narudzba narudzba)
        {
            int idNarudzbe = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "insert into narudzba (datum, ukupna_cijena, dobavljac_iddobavljac) values (@datum, @ukupna_cijena, @dobavljac_iddobavljac); SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@datum", narudzba.datum);
                    command.Parameters.AddWithValue("@ukupna_cijena", narudzba.ukupna_cijena);
                    command.Parameters.AddWithValue("@dobavljac_iddobavljac", narudzba.dobavljac_iddobavljac);

                    connection.Open();
                    idNarudzbe = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return idNarudzbe;
        }

        public static List<Narudzba> GetAllNarudzbe()
        {
            List<Narudzba> narudzbaList = new List<Narudzba>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("select idnarudzba, datum, ukupna_cijena, dobavljac_iddobavljac from narudzba", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Narudzba narudzbe = new Narudzba
                    {
                        idnarudzba = reader.GetInt32(0),
                        datum=reader.GetDateTime(1),
                        ukupna_cijena=reader.GetDecimal(2),
                        dobavljac_iddobavljac=reader.GetInt32(3),
                    };
                    narudzbaList.Add(narudzbe);
                }
            }

            return narudzbaList;
        }
    }
}
