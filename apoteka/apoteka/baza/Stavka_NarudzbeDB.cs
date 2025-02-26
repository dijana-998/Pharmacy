using apoteka.model;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace apoteka.baza
{
    internal class Stavka_NarudzbeDB
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ApotekaDBConnection"].ConnectionString;

        public static bool DodajStavkuNarudzbe(Stavka_Narudzbe stavka)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "insert into stavka_narudzbe (kolicina, cijena, narudzba_idnarudzba, artikal_idartikal) values (@kolicina, @cijena, @narudzba_idnarudzba, @artikal_idartikal)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@kolicina", stavka.kolicina);
                    command.Parameters.AddWithValue("@cijena", stavka.cijena);
                    command.Parameters.AddWithValue("@narudzba_idnarudzba", stavka.narudzba_idnarudzba);
                    command.Parameters.AddWithValue("@artikal_idartikal", stavka.artikal_idartikal);

                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
