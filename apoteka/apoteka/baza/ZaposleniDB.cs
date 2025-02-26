using apoteka.model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

namespace apoteka.baza
{
    internal class ZaposleniDB
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ApotekaDBConnection"].ConnectionString;

        public (bool isAuthenticated, Zaposleni? zaposleni) Autentikacija(string korisnickoIme, string lozinka)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                
                string query = "select idzaposleni, jmb, ime, prezime, korisnicko_ime, lozinka, uloga, datum_zaposlenja, datum_prestanka, tema_idtema, jezik_idjezik from zaposleni where korisnicko_ime = @korisnickoIme and lozinka = @lozinka";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@korisnickoIme", korisnickoIme);
                    command.Parameters.AddWithValue("@lozinka", lozinka);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Zaposleni zaposleni = new Zaposleni
                            {
                                idzaposleni = reader.GetInt32("idzaposleni"),
                                jmb = reader.GetString("jmb"),
                                ime = reader.GetString("ime"),
                                prezime = reader.GetString("prezime"),
                                korisnicko_ime = reader.GetString("korisnicko_ime"),
                                lozinka = reader.GetString("lozinka"),
                                uloga = reader.GetString("uloga"),
                                datum_zaposlenja = reader.GetDateTime("datum_zaposlenja"),
                                datum_prestanka = reader.GetDateTime("datum_prestanka"),
                                tema_idtema = reader.GetInt32("tema_idtema"),
                                jezik_idjezik = reader.GetInt32("jezik_idjezik")
                            };
                            return (true, zaposleni);
                        }
                    }
                }
            }
            return (false, null);
        }

        public static List<Zaposleni> GetAllZaposleni()
        {
            List<Zaposleni> zaposleniList = new List<Zaposleni>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("select idzaposleni, jmb, ime, prezime, korisnicko_ime, lozinka, uloga, datum_zaposlenja, datum_prestanka, tema_idtema, jezik_idjezik from zaposleni", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Zaposleni zaposleni = new Zaposleni
                    {
                        idzaposleni = reader.GetInt32(0),
                        jmb = reader.IsDBNull(1) ? null : reader.GetString(1),
                        ime = reader.IsDBNull(2) ? null : reader.GetString(2),
                        prezime = reader.IsDBNull(3) ? null : reader.GetString(3),
                        korisnicko_ime = reader.IsDBNull(4) ? null : reader.GetString(4),
                        lozinka=reader.IsDBNull(5) ? null:reader.GetString(5),
                        uloga = reader.IsDBNull(6) ? null : reader.GetString(6),
                        datum_zaposlenja = reader.GetDateTime(7),
                        datum_prestanka = reader.GetDateTime(8),
                        jezik_idjezik=reader.GetInt32(9),
                        tema_idtema = reader.GetInt32(10)
                    };
                    zaposleniList.Add(zaposleni);
                }
            }
            return zaposleniList;
        }

        public static bool DodajZaposlenog(Zaposleni zaposleni)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "insert into zaposleni (jmb, ime, prezime, korisnicko_ime, lozinka, uloga, datum_zaposlenja, datum_prestanka, tema_idtema, jezik_idjezik) " +
                                   "values (@jmb, @ime, @prezime, @korisnicko_ime, @lozinka, @uloga, @datum_zaposlenja, @datum_prestanka, @tema_idtema, @jezik_idjezik)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@jmb", zaposleni.jmb);
                        command.Parameters.AddWithValue("@ime", zaposleni.ime);
                        command.Parameters.AddWithValue("@prezime", zaposleni.prezime);
                        command.Parameters.AddWithValue("@korisnicko_ime", zaposleni.korisnicko_ime);
                        command.Parameters.AddWithValue("@lozinka", zaposleni.lozinka);
                        command.Parameters.AddWithValue("@uloga", zaposleni.uloga);
                        command.Parameters.AddWithValue("@datum_zaposlenja", zaposleni.datum_zaposlenja);
                        command.Parameters.AddWithValue("@datum_prestanka", zaposleni.datum_prestanka);
                        command.Parameters.AddWithValue("@tema_idtema", zaposleni.tema_idtema);
                        command.Parameters.AddWithValue("@jezik_idjezik", zaposleni.jezik_idjezik);

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

        public static bool ObrisiZaposlenog(int idzaposleni)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "delete from zaposleni where idzaposleni = @idzaposleni";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idzaposleni", idzaposleni);
                        int affectedRows = command.ExecuteNonQuery();

                        
                        return affectedRows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške pri brisanju zaposlenog: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool AzurirajZaposlenog(Zaposleni zaposleni)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"update zaposleni 
                             set ime = @ime, 
                                 prezime = @prezime, 
                                 korisnicko_ime = @korisnickoIme, 
                                 lozinka = @lozinka, 
                                 datum_zaposlenja = @datum_zaposlenja, 
                                 datum_prestanka = @datum_prestanka
                             where idzaposleni = @idzaposleni";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@ime", zaposleni.ime);
                        command.Parameters.AddWithValue("@prezime", zaposleni.prezime);
                        command.Parameters.AddWithValue("@korisnickoIme", zaposleni.korisnicko_ime);
                        command.Parameters.AddWithValue("@lozinka", zaposleni.lozinka);
                        command.Parameters.AddWithValue("@datum_zaposlenja", zaposleni.datum_zaposlenja);
                        command.Parameters.AddWithValue("@datum_prestanka", zaposleni.datum_prestanka);
                        command.Parameters.AddWithValue("@idzaposleni", zaposleni.idzaposleni);

                        
                        int affectedRows = command.ExecuteNonQuery();

                        
                        return affectedRows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške pri ažuriranju zaposlenog: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool azurirajLozinku(int? idzaposleni, string novaLozinka)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    
                    string query = "update zaposleni set lozinka = @novaLozinka where idzaposleni = @idzaposleni";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@novaLozinka", novaLozinka);
                        command.Parameters.AddWithValue("@idzaposleni", idzaposleni);

                        
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Greška prilikom ažuriranja lozinke: {ex.Message}");
                return false;
            }
        }

        public bool AzurirajTemu(int? idzaposleni,int tema_idtema)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "update zaposleni set tema_idtema = @tema_idtema where idzaposleni = @idzaposleni";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tema_idtema", tema_idtema);
                        command.Parameters.AddWithValue("@idzaposleni", idzaposleni);
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom ažuriranja teme: " + ex.Message);
                    return false;
                }
            }
        }

        public bool AzurirajJezik(int? idzaposleni, int jezik_idjezik)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "update zaposleni set jezik_idjezik = @jezik_idjezik where idzaposleni = @idzaposleni";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@jezik_idjezik", jezik_idjezik);
                        command.Parameters.AddWithValue("@idzaposleni", idzaposleni);
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom ažuriranja jezika: " + ex.Message);
                    return false;
                }
            }
        }

        public static int? DohvatiIdTemeZaZaposlenog(int idZaposlenog)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
       
                    string query = "select tema_idtema from zaposleni where idzaposleni = @idzaposleni";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idzaposleni", idZaposlenog);

                        
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return null; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom dohvatanja ID teme za zaposlenog: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static int? DohvatiIdJezikaZaZaposlenog(int idZaposlenog)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "select jezik_idjezik from zaposleni where idzaposleni = @idzaposleni";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idzaposleni", idZaposlenog);


                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom dohvatanja ID jezika za zaposlenog: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

    }
}