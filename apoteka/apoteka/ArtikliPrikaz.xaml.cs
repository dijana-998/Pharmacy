using apoteka.baza;
using apoteka.model;
using System;
using System.Windows;
using System.Windows.Input;

namespace apoteka
{
    public partial class ArtikliPrikaz : Window
    {
        private string slikaPutanja="";
        Zaposleni zaposleniPrikaz;
        public ArtikliPrikaz(Zaposleni zaposleniPrikaz)
        {
            InitializeComponent();
            ucitajArtikle();
            this.zaposleniPrikaz = zaposleniPrikaz;
            PromeniTemu();
            PromeniJezik();
        }

        private void PromeniJezik()
        {
            if(zaposleniPrikaz.jezik_idjezik==61)
            {
                artikliTextBlock.Text = "Artikli";
                artikliDataGrid.Columns[0].Header = "IdArtikal";
                artikliDataGrid.Columns[1].Header = "Naziv";
                artikliDataGrid.Columns[2].Header = "Cijena";
                artikliDataGrid.Columns[3].Header = "Kolicina";
                artikliDataGrid.Columns[4].Header = "Rok trajanja";
                DodajArtikaTextBlock.Text = "Dodaj artikal:";
                nazivTextBlock.Text = "Naziv:";
                OpisTextBlock.Text = "Opis:";
                cijenaTextBlock.Text = "Cijena:";
                kolicinaTextBlock.Text = "Kolicina:";
                rokTrajanjaTextBlock.Text = "Rok trajanja:";
                DodajSlikuTextBlock.Text = "Dodaj sliku:";
            }
            else
            {
                artikliTextBlock.Text = "Items";
                artikliDataGrid.Columns[0].Header = "Item ID";
                artikliDataGrid.Columns[1].Header = "Name";
                artikliDataGrid.Columns[2].Header = "Price";
                artikliDataGrid.Columns[3].Header = "Quantity";
                artikliDataGrid.Columns[4].Header = "Expiration date";
                DodajArtikaTextBlock.Text = "Add item:";
                nazivTextBlock.Text = "Name:";
                OpisTextBlock.Text = "Description:";
                cijenaTextBlock.Text = "Price:";
                kolicinaTextBlock.Text = "Quantity:";
                rokTrajanjaTextBlock.Text = "Expiration Date:";
                DodajSlikuTextBlock.Text = "Add image:";
            }
        }
        private void PromeniTemu()
        {
            App aplikacija = (App)Application.Current;
            aplikacija.PromijeniTemu(zaposleniPrikaz.tema_idtema);
        }

        public void ucitajArtikle()
        {
            var artikli = ArtikliDB.GetAllArtikli();
            artikliDataGrid.ItemsSource = artikli;
        }

        private void artikliDataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (artikliDataGrid.SelectedItem is Artikli selektovaniArtikal)
            {
                ArtikalInfo info = new ArtikalInfo(selektovaniArtikal, this,zaposleniPrikaz);
                info.ShowDialog();
            }
            else
            {
                MessageBox.Show("Došlo je do greške pri otvaranju prozora za informacije o artiklu.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dodajArtikal_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nazivTextBox.Text))
            {
                MessageBox.Show("Naziv mora biti popunjen.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(opisTextBox.Text))
            {
                MessageBox.Show("Opis mora biti popunjen.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal? cijena;

            if(decimal.TryParse(cijenaTextBox.Text,out var parsedCijena))
            {
                cijena = parsedCijena;
            }
            else
            {
                MessageBox.Show("Cijena mora biti decimalni broj.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int kolicina;

            if (int.TryParse(kolicinaTextBox.Text,out var parsedKolicina))
            {
                kolicina = parsedKolicina;
            }
            else
            {
                MessageBox.Show("Količina mora biti cijeli broj.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (RokTrajanjaDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Rok trajanja mora biti odabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(slikaPutanja))
            {
                MessageBox.Show("Morate izabrati sliku za artikal.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var noviArtikal = new Artikli
            {
                naziv=nazivTextBox.Text,
                opis=opisTextBox.Text,
                cijena = cijena,
                kolicina= kolicina,
                rok_trajanja = RokTrajanjaDatePicker.SelectedDate.Value
            };

           
            bool uspesnoDodato = ArtikliDB.DodajArtikal(noviArtikal);

            if (uspesnoDodato)
            {
                int idArtikla = ArtikliDB.GetLastInsertedId();
                if (!string.IsNullOrEmpty(slikaPutanja))
                {
                    try
                    {
                        
                        string folderSlike = @"apoteka\apoteka\slike";
                        if (!System.IO.Directory.Exists(folderSlike))
                        {
                            System.IO.Directory.CreateDirectory(folderSlike);
                        }

                        
                        string destinacija = System.IO.Path.Combine(folderSlike, $"{idArtikla}.jpg");

                        
                        System.IO.File.Copy(slikaPutanja, destinacija, true);
                        MessageBox.Show("Slika je uspešno sačuvana.", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Došlo je do greške pri čuvanju slike: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                MessageBox.Show("Artikal je uspješno dodat.", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
                ucitajArtikle(); 
            }
            else
            {
                MessageBox.Show("Došlo je do greške pri dodavanju artikla.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ocistiSvaPolja();
        }

        private void povratak_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (artikliDataGrid.SelectedItem is Artikli selektovaniArtikal)
            {
                
                var rezultat = MessageBox.Show($"Da li ste sigurni da želite obrisati artikal {selektovaniArtikal.naziv}?",
                                               "Potvrda brisanja",
                                               MessageBoxButton.YesNo,
                                               MessageBoxImage.Question);

                if (rezultat == MessageBoxResult.Yes)
                {
                    
                    bool uspesnoObrisano = ArtikliDB.ObrisiArtikal(selektovaniArtikal.idartikal);

                    if (uspesnoObrisano)
                    {
                        MessageBox.Show("Artikal je uspešno obrisan.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                        ucitajArtikle(); 
                    }
                    else
                    {
                        MessageBox.Show("Došlo je do greške pri brisanju artikla.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Niste odabrali artikal za brisanje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            ucitajArtikle();
        }
        private void ocistiSvaPolja()
        {
            nazivTextBox.Text = "";
            opisTextBox.Text = "";
            cijenaTextBox.Text = "";
            kolicinaTextBox.Text = "";
            RokTrajanjaDatePicker.Text = null;
        }

        private void dodajSliku_Click(object sender, RoutedEventArgs e)
        {
            
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Izaberite sliku artikla"
            };

            
            if (openFileDialog.ShowDialog() == true)
            {
                
                slikaPutanja = openFileDialog.FileName;
                MessageBox.Show("Slika je uspešno izabrana.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Niste izabrali sliku.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
