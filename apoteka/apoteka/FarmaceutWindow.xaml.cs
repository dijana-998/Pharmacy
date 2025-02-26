using apoteka.baza;
using apoteka.model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace apoteka
{

    public partial class FarmaceutWindow : Window
    {
        Zaposleni zaposleni;
        private decimal ukupna_cijena;
        private ObservableCollection<Artikli> artikli;
        public FarmaceutWindow(Zaposleni zaposleniFarmaceut)
        {
            InitializeComponent();
            this.zaposleni = zaposleniFarmaceut;
            artikli = new ObservableCollection<Artikli>();
            artikliDataGrid.ItemsSource = artikli;
            ukupna_cijena = 0;
            PromeniTemu();
            PromeniJezik();
            App.JezikPromenjen += PromeniJezik;
        }
        private void PromeniTemu()
        {
            App aplikacija = (App)Application.Current;
            aplikacija.PromijeniTemu(zaposleni.tema_idtema);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.JezikPromenjen -= PromeniJezik;
        }

        private void PromeniJezik()
        {
            if(zaposleni.jezik_idjezik==61)
            {
                farmaceutTextBlockTitle.Text = "Farmaceut";
                farmaceutTextBlock.Text = "Farmaceut";
                prodajaTextBlock.Text = "Prodaja";
                idartiklaTextBlock.Text = "IdArtikla";
                nazivTextBlock.Text = "Naziv";
                kolicinaTextBlock.Text = "Kolicina";
                artikliDataGrid.Columns[0].Header = "Naziv";
                artikliDataGrid.Columns[1].Header = "Cijena";
                artikliDataGrid.Columns[2].Header = "Kolicina";
                artikliDataGrid.Columns[3].Header = "Rok trajanja";
                datumProdajeTextBlock.Text = "Datum prodaje:";
                ukupnoTextBlock.Text = "Ukupno:";
            }
            else
            {
                farmaceutTextBlockTitle.Text = "Pharmacist";
                farmaceutTextBlock.Text = "Pharmacist";
                prodajaTextBlock.Text = "Sale";
                idartiklaTextBlock.Text = "Items ID";
                nazivTextBlock.Text = "Name";
                kolicinaTextBlock.Text = "Quantity";
                artikliDataGrid.Columns[0].Header = "Name";
                artikliDataGrid.Columns[1].Header = "Price";
                artikliDataGrid.Columns[2].Header = "Quantity";
                artikliDataGrid.Columns[3].Header = "Expiration Date";
                datumProdajeTextBlock.Text = "Date of sale:";
                ukupnoTextBlock.Text = "Total:";
            }
        }

        private void azurirajUkupno()
        {
            ukupnoTextBox.Text = ukupna_cijena.ToString("F2"); 
        }

        private void idArtiklaTextChanged(object sender, TextChangedEventArgs e)
        {
            // Provera da li je unet validan broj za ID
            if (int.TryParse(idArtiklaTextBox.Text, out int idArtikla))
            {
                // Pronalaženje artikla u bazi
                Artikli? artikal = ArtikliDB.PronadjiArtikalUBazi(idArtikla);

                if (artikal != null)
                {
                    nazivArtiklaTextBox.Text = artikal.naziv;
                }
                else
                {
                    nazivArtiklaTextBox.Text = "";
                }
            }
            else
            {
                nazivArtiklaTextBox.Text = "";
            }
        }

        private void dodajButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(idArtiklaTextBox.Text, out int idArtikla))
            {
                MessageBox.Show("Molimo unesite ispravan ID artikla.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(kolicinaArtiklaTextBox.Text, out int kolicina) || kolicina <= 0)
            {
                MessageBox.Show("Molimo unesite ispravnu količinu (mora biti pozitivan broj).", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Artikli? pronadeniArtikal = ArtikliDB.PronadjiArtikalUBazi(idArtikla);
            if (pronadeniArtikal == null)
            {
                MessageBox.Show("Artikal sa unetim ID-jem ne postoji u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal cijenaArtikla = pronadeniArtikal.cijena.GetValueOrDefault() * kolicina;

            var noviArtikal = new Artikli
            {
                idartikal = pronadeniArtikal.idartikal,
                naziv = pronadeniArtikal.naziv,
                cijena = cijenaArtikla,
                kolicina = kolicina,
                rok_trajanja = pronadeniArtikal.rok_trajanja
            };

            
            artikli.Add(noviArtikal);

            ukupna_cijena += cijenaArtikla;
            azurirajUkupno(); 
            ocistiPolja();
        }

        private void ocistiPolja()
        {
            idArtiklaTextBox.Text = "";
            nazivArtiklaTextBox.Text = "";
            kolicinaArtiklaTextBox.Text = "";
        }

        private void obrisiButton_Click(object sender, RoutedEventArgs e)
        {
           
            var selektovaniRed = (Artikli)artikliDataGrid.SelectedItem;
            if (selektovaniRed == null)
            {
                MessageBox.Show("Molimo odaberite artikal koji želite da obrišete.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            decimal cijenaArtikla = selektovaniRed.cijena.GetValueOrDefault();

           
            artikli.Remove(selektovaniRed);

            
            ukupna_cijena -= cijenaArtikla;

            
            azurirajUkupno();
        }

        private void potvrdiProdajuButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (datumProdajePicker.SelectedDate == null)
            {
                MessageBox.Show("Molimo izaberite datum za prodaje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (artikli == null || artikli.Count == 0)
            {
                MessageBox.Show("Molimo dodajte barem jedan artikal u prodaju pre potvrđivanja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            DateTime datumProdaje = datumProdajePicker.SelectedDate.Value;

           
            Racun noviRacun = new Racun(datumProdaje, ukupna_cijena, zaposleni.idzaposleni);
            int idRacun = apoteka.baza.RacunDB.DodajRacun(noviRacun);

            if (idRacun == -1)
            {
                MessageBox.Show("Došlo je do greške pri dodavanju prodaje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            foreach (var artikal in artikli)
            {
                StavkaRacuna stavka = new StavkaRacuna
                {
                    kolicina = artikal.kolicina,
                    cijena = artikal.cijena,
                    racun_idracun = idRacun,
                    artikal_idartikal = artikal.idartikal
                };

                bool uspesnoDodavanje = apoteka.baza.StavkaRacunaDB.DodajStavkuRacuna(stavka);

                if (!uspesnoDodavanje)
                {
                    MessageBox.Show("Došlo je do greške pri dodavanju stavke racuna.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            MessageBox.Show("Prodaja je uspješno dodata.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

            
            artikli.Clear();
            ukupna_cijena = 0;
            azurirajUkupno();
            ocistiSvaPolja();
        }

        private void ocistiSvaPolja()
        {
            datumProdajePicker.Text = "";
            ocistiPolja();
        }

        private void prikaziProdaju_Click(object sender, EventArgs e)
        {
            ProdajaPrikaz proPrikaz = new ProdajaPrikaz(zaposleni);
            proPrikaz.Show();
        }

        private void artikli_Click(object sender, EventArgs e)
        {
            ArtikliPrikaz artPrikaz = new ArtikliPrikaz(zaposleni);
            artPrikaz.Show();
        }

        private void podesavanja_Click(object sender, EventArgs e)
        {
            PodesavanjaPrikaz podPrikaz = new PodesavanjaPrikaz(zaposleni);
            podPrikaz.Show();
        }

        private void odjava_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
