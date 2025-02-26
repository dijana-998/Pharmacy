using apoteka.model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;

namespace apoteka.baza
{
    class ArtikliDB
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ApotekaDBConnection"].ConnectionString;

        public static List<Artikli> GetAllArtikli()
        {
            List<Artikli> artikliList = new List<Artikli>();

            using (MySqlConnection connection = new MySqlConnection(connectionString)) 
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("select idartikal, naziv, opis, cijena, kolicina, rok_trajanja from artikal", connection); 
                MySqlDataReader reader = command.ExecuteReader(); 

                while (reader.Read())
                {
                    Artikli artikal = new Artikli
                    {
                        idartikal = reader.GetInt32(0),
                        naziv = reader.IsDBNull(1) ? null : reader.GetString(1),
                        opis = reader.IsDBNull(2) ? null : reader.GetString(2),
                        cijena = reader.IsDBNull(3) ? (decimal?)null : reader.GetDecimal(3),
                        kolicina = reader.GetInt32(4),
                        rok_trajanja = reader.GetDateTime(5)
                    };
                    artikliList.Add(artikal);
                }
            }

            return artikliList;
        }

        public static Artikli? PronadjiArtikalUBazi(int idArtikla)
        {
            Artikli? artikal = null;

            using(MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "select idartikal, naziv, opis, cijena, kolicina, rok_trajanja from artikal where idartikal=@idArtikla";
                    using(MySqlCommand command= new MySqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@idArtikla", idArtikla);

                        using(MySqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                artikal = new Artikli
                                {
                                    idartikal = reader.GetInt32("idartikal"),
                                    naziv = reader.IsDBNull("naziv") ? null : reader.GetString("naziv"),
                                    opis = reader.IsDBNull("opis") ? null : reader.GetString("opis"),
                                    cijena = reader.IsDBNull("cijena") ? (decimal?)null : reader.GetDecimal("cijena"),
                                    kolicina = reader.GetInt32("kolicina"),
                                    rok_trajanja = reader.GetDateTime("rok_trajanja")
                                };
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Greška pri pretrazi artikla: " + ex.Message);
                }
            }
            return artikal;
        }

        public static bool DodajArtikal(Artikli artikli)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "insert into artikal (naziv, opis, cijena, kolicina, rok_trajanja) " +
                                   "values (@naziv, @opis, @cijena, @kolicina, @rok_trajanja)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@naziv", artikli.naziv);
                        command.Parameters.AddWithValue("@opis", artikli.opis);
                        command.Parameters.AddWithValue("@cijena", artikli.cijena);
                        command.Parameters.AddWithValue("@kolicina", artikli.kolicina);
                        command.Parameters.AddWithValue("@rok_trajanja", artikli.rok_trajanja);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool ObrisiArtikal(int idartikal)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "delete from artikal where idartikal = @idartikal";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idartikal", idartikal);
                        int affectedRows = command.ExecuteNonQuery();

                        return affectedRows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške pri brisanju artikla: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool AzurirajArtikal(Artikli artikal)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"update artikal 
                             set naziv = @naziv, 
                                 opis = @opis, 
                                 cijena = @cijena, 
                                 kolicina = @kolicina, 
                                 rok_trajanja = @rok_trajanja 
                             where idartikal = @idartikal";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@naziv", artikal.naziv);
                        command.Parameters.AddWithValue("@opis", artikal.opis);
                        command.Parameters.AddWithValue("@cijena", artikal.cijena);
                        command.Parameters.AddWithValue("@kolicina", artikal.kolicina);
                        command.Parameters.AddWithValue("@rok_trajanja", artikal.rok_trajanja);
                        command.Parameters.AddWithValue("@idartikal", artikal.idartikal);

                        
                        int affectedRows = command.ExecuteNonQuery();

                        
                        return affectedRows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške pri ažuriranju artikla: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static int GetLastInsertedId()
        {
            int lastId = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT LAST_INSERT_ID()";
                using (var command = new MySqlCommand(query, connection))
                {
                    lastId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return lastId;
        }

    }
}
