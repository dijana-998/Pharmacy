using apoteka.model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apoteka.baza
{
    internal class RacunDB
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ApotekaDBConnection"].ConnectionString;

        public static int DodajRacun(Racun racun)
        {
            int idRacun = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "insert into racun (datum, ukupan_iznos, zaposleni_idzaposleni) values (@datum, @ukupan_iznos, @zaposleni_idzaposleni); SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@datum", racun.datum);
                    command.Parameters.AddWithValue("@ukupan_iznos", racun.ukupan_iznos);
                    command.Parameters.AddWithValue("@zaposleni_idzaposleni", racun.zaposleni_idzaposleni);

                    connection.Open();
                    idRacun = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return idRacun;
        }

        public static List<Racun> GetAllRacuni()
        {
            List<Racun> racuniList = new List<Racun>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("select idracun, datum, ukupan_iznos, zaposleni_idzaposleni from racun", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Racun racuni = new Racun
                    {
                        idracun = reader.GetInt32(0),
                        datum = reader.GetDateTime(1),
                        ukupan_iznos = reader.GetDecimal(2),
                        zaposleni_idzaposleni = reader.GetInt32(3),
                    };
                    racuniList.Add(racuni);
                }
            }

            return racuniList;
        }
    }
}
