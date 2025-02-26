using apoteka.baza;
using apoteka.model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace apoteka
{
    public partial class AdministratorWindow : Window
    {
        private ObservableCollection<Artikli> artikli;
        private decimal ukupna_cijena;
        Zaposleni prijavljeniZaposleni;
        public AdministratorWindow(Zaposleni zaposleni)
        {
            InitializeComponent();
            artikli = new ObservableCollection<Artikli>();
            artikliDataGrid.ItemsSource = artikli;
            ucitajDobavljace();
            ukupna_cijena = 0;
            this.prijavljeniZaposleni = zaposleni;
            PromeniTemu();
            PromeniJezik();
            App.JezikPromenjen += PromeniJezik;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.JezikPromenjen-= PromeniJezik;
        }

        private void PromeniJezik()
        {
            if(prijavljeniZaposleni.jezik_idjezik==61)
            {
                narudzbaTextBlock.Text = "Narudzba";
                idArtiklaTextBlock.Text = "IdArtikla";
                nazivTextBlock.Text = "Naziv";
                kolicinaTextBlock.Text = "Kolicina";
                artikliDataGrid.Columns[0].Header = "Naziv";
                artikliDataGrid.Columns[1].Header = "Cijena";
                artikliDataGrid.Columns[2].Header = "Kolicina";
                artikliDataGrid.Columns[3].Header = "Rok trajanja";
                datumNarudzbeTextBlock.Text = "Datum narudzbe:";
                dobavljaciDataGrid.Columns[0].Header = "IdDobavljaca";
                dobavljaciDataGrid.Columns[1].Header = "Naziv";
                dobavljaciDataGrid.Columns[2].Header = "Telefon";
                dobavljaciDataGrid.Columns[3].Header = "Adresa";
                dobavljaciDataGrid.Columns[4].Header = "Email";
                UkupnoTextBlock.Text = "Ukupno:";
            }
            else
            {
                narudzbaTextBlock.Text = "Order";
                idArtiklaTextBlock.Text = "Item ID";
                nazivTextBlock.Text = "Name";
                kolicinaTextBlock.Text = "Quantity";
                artikliDataGrid.Columns[0].Header = "Name";
                artikliDataGrid.Columns[1].Header = "Price";
                artikliDataGrid.Columns[2].Header = "Quantity";
                artikliDataGrid.Columns[3].Header = "Expiration Date";
                datumNarudzbeTextBlock.Text = "Date of order:";
                dobavljaciDataGrid.Columns[0].Header = "Supplier ID";
                dobavljaciDataGrid.Columns[1].Header = "Name";
                dobavljaciDataGrid.Columns[2].Header = "Phone";
                dobavljaciDataGrid.Columns[3].Header = "Adress";
                dobavljaciDataGrid.Columns[4].Header = "Email";
                UkupnoTextBlock.Text = "Total:";
            }
        }

        private void PromeniTemu()
        {
            App aplikacija = (App)Application.Current;
            aplikacija.PromijeniTemu(prijavljeniZaposleni.tema_idtema);
        }

        private void azurirajUkupno()
        {
            ukupnoTextBox.Text = ukupna_cijena.ToString("F2"); 
        }

        private void ucitajDobavljace()
        {
            var dobavljaci = DobavljacDB.GetAllDobavljaci();
            dobavljaciDataGrid.ItemsSource = dobavljaci;
        }

        private void idArtiklaTextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (int.TryParse(idArtiklaTextBox.Text, out int idArtikla))
            {
                
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
                MessageBox.Show("Molimo unesite ispravan ID artikla.","Greška",MessageBoxButton.OK,MessageBoxImage.Error);
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
                rok_trajanja=pronadeniArtikal.rok_trajanja
            };

            
            artikli.Add(noviArtikal);

            ukupna_cijena += cijenaArtikla;
            azurirajUkupno(); 
            ocistiPolja();
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

        private void potvrdiNarudzbuButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (datumNarudzbePicker.SelectedDate == null)
            {
                MessageBox.Show("Molimo izaberite datum za narudžbu.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            if (dobavljaciDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Molimo izaberite dobavljača za narudžbu.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (artikli == null || artikli.Count == 0)
            {
                MessageBox.Show("Molimo dodajte barem jedan artikal u narudžbu pre potvrđivanja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            DateTime datumNarudzbe = datumNarudzbePicker.SelectedDate.Value;
            var selektovaniDobavljac = (Dobavljac)dobavljaciDataGrid.SelectedItem;
            int idDobavljaca = selektovaniDobavljac.iddobavljac;

            
            Narudzba novaNarudzba = new Narudzba(datumNarudzbe, ukupna_cijena, idDobavljaca);
            int idNarudzbe = apoteka.baza.NarudzbaDB.DodajNarudzbu(novaNarudzba);

            if (idNarudzbe == -1)
            {
                MessageBox.Show("Došlo je do greške pri dodavanju narudžbe.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

           
            foreach (var artikal in artikli)
            {
                Stavka_Narudzbe stavka = new Stavka_Narudzbe
                {
                    kolicina = artikal.kolicina,
                    cijena = artikal.cijena,
                    narudzba_idnarudzba = idNarudzbe,
                    artikal_idartikal = artikal.idartikal
                };

                bool uspesnoDodavanje = apoteka.baza.Stavka_NarudzbeDB.DodajStavkuNarudzbe(stavka);

                if (!uspesnoDodavanje)
                {
                    MessageBox.Show("Došlo je do greške pri dodavanju stavke narudžbe.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            MessageBox.Show("Narudžba je uspešno dodata.", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

            
            artikli.Clear();
            ukupna_cijena = 0;
            azurirajUkupno();
            ocistiSvaPolja();
        }

        private void ocistiPolja()
        {
            idArtiklaTextBox.Text = "";
            nazivArtiklaTextBox.Text = "";
            kolicinaArtiklaTextBox.Text = "";
        }

        private void ocistiSvaPolja()
        {
            datumNarudzbePicker.Text = "";
            ocistiPolja();
        }

        private void prikazNarudzbe_Click(object sender, EventArgs e)
        {
            NarudzbaPrikaz narPrikaz = new NarudzbaPrikaz(prijavljeniZaposleni);
            narPrikaz.Show();
        }

        private void zaposleni_Click(object sender, EventArgs e)
        {
            ZaposleniPrikaz zapPrikaz = new ZaposleniPrikaz(prijavljeniZaposleni);
            zapPrikaz.Show();
        }

        private void artikli_Click(object sender, EventArgs e)
        {
            ArtikliPrikaz artPrikaz = new ArtikliPrikaz(prijavljeniZaposleni);
            artPrikaz.Show();
        }

        private void podesavanja_Click(object sender, EventArgs e)
        {
            PodesavanjaPrikaz podPrikaz = new PodesavanjaPrikaz(prijavljeniZaposleni);
            podPrikaz.Show();
        }

        private void odjava_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}