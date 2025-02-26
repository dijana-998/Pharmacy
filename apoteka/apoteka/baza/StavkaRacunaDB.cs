using apoteka.model;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace apoteka.baza
{
    internal class StavkaRacunaDB
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ApotekaDBConnection"].ConnectionString;

        public static bool DodajStavkuRacuna(StavkaRacuna stavka)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "insert into stavka_racuna (kolicina, cijena, racun_idracun, artikal_idartikal) values (@kolicina, @cijena, @racun_idracun, @artikal_idartikal)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@kolicina", stavka.kolicina);
                    command.Parameters.AddWithValue("@cijena", stavka.cijena);
                    command.Parameters.AddWithValue("@racun_idracun", stavka.racun_idracun);
                    command.Parameters.AddWithValue("@artikal_idartikal", stavka.artikal_idartikal);

                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
