using apoteka.baza;
using apoteka.model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace apoteka
{

    public partial class ZaposleniPrikaz : Window
    {
        Zaposleni zaposleniPrikaz;
        public ZaposleniPrikaz(Zaposleni zaposleniPrikaz)
        {
            InitializeComponent();
            ucitajZaposlene();
            this.zaposleniPrikaz = zaposleniPrikaz;
            PromeniTemu();
            PromeniJezik();
        }
        private void PromeniTemu()
        {
            App aplikacija = (App)Application.Current;
            aplikacija.PromijeniTemu(zaposleniPrikaz.tema_idtema);
        }

        private void PromeniJezik()
        {
           if(zaposleniPrikaz.jezik_idjezik==61)
            {
                ZaposleniTitle.Text = "Zaposleni";
                zaposleniDataGrid.Columns[0].Header = "IdZaposlenog";
                zaposleniDataGrid.Columns[1].Header = "JMB";
                zaposleniDataGrid.Columns[2].Header = "Ime";
                zaposleniDataGrid.Columns[3].Header = "Prezime";
                zaposleniDataGrid.Columns[4].Header = "Datum zaposlenja";
                zaposleniDataGrid.Columns[5].Header = "Datum prestanka";
                dodajteZaposlenogTextBlock.Text = "Dodajte zaposlenog:";
                jmbTextBlock.Text = "JMB:";
                imeTextBlock.Text = "Ime:";
                prezimeTextBlock.Text = "Prezime:";
                korisnickoImeTextBlock.Text = "Korisnicko ime:";
                lozinkaTextBlock.Text = "Lozinka:";
                ulogaTextBlock.Text = "Uloga:";
                datumZaposlenjaTextBlock.Text = "Datum zaposlenja:";
                datumPrestankaTextBlock.Text = "Datum prestanka:";

            }
            else
            {
                ZaposleniTitle.Text = "Employed";
                zaposleniDataGrid.Columns[0].Header = "Employee ID";
                zaposleniDataGrid.Columns[1].Header = "UIN";
                zaposleniDataGrid.Columns[2].Header = "Name";
                zaposleniDataGrid.Columns[3].Header = "Surname";
                zaposleniDataGrid.Columns[4].Header = "Employment Date";
                zaposleniDataGrid.Columns[5].Header = "Termination Date";
                dodajteZaposlenogTextBlock.Text = "Add employee:";
                jmbTextBlock.Text = "UIN:";
                imeTextBlock.Text = "Name:";
                prezimeTextBlock.Text = "Surname:";
                korisnickoImeTextBlock.Text = "Username:";
                lozinkaTextBlock.Text = "Password:";
                ulogaTextBlock.Text = "Role:";
                datumZaposlenjaTextBlock.Text = "Employment Date:";
                datumPrestankaTextBlock.Text = "Termination Date:";
            }
        }

        public void ucitajZaposlene()
        {
            var zaposleni = ZaposleniDB.GetAllZaposleni();
            zaposleniDataGrid.ItemsSource = zaposleni;
        }

        private void zaposleniDataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (zaposleniDataGrid.SelectedItem is Zaposleni selektovaniZaposleni)
            {
                ZaposleniInfo info = new ZaposleniInfo(selektovaniZaposleni,this,zaposleniPrikaz);
                info.ShowDialog();
            }
            else
            {
                MessageBox.Show("Došlo je do greške pri otvaranju prozora za informacije o zaposlenom.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dodajZaposlenog_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(jmbTextBox.Text) || jmbTextBox.Text.Length != 13 || !long.TryParse(jmbTextBox.Text, out _))
            {
                MessageBox.Show("JMB mora imati tačno 13 cifara.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(imeTextBox.Text))
            {
                MessageBox.Show("Ime mora biti popunjeno.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(prezimeTextBox.Text))
            {
                MessageBox.Show("Prezime mora biti popunjeno.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(korisnickoImeTextBox.Text))
            {
                MessageBox.Show("Korisničko ime mora biti popunjeno.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(lozinkaTextBox.Text))
            {
                MessageBox.Show("Lozinka mora biti popunjena.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ulogaComboBox.SelectedItem == null)
            {
                MessageBox.Show("Morate odabrati ulogu.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (datumZaposlenjaDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Datum zaposlenja mora biti odabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (datumPrestankaDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Datum prestanka mora biti odabran.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            var noviZaposleni = new Zaposleni
            {
                jmb = jmbTextBox.Text,
                ime = imeTextBox.Text,
                prezime = prezimeTextBox.Text,
                korisnicko_ime = korisnickoImeTextBox.Text,
                lozinka = lozinkaTextBox.Text,
                uloga = ((ComboBoxItem)ulogaComboBox.SelectedItem).Content.ToString(),
                datum_zaposlenja = datumZaposlenjaDatePicker.SelectedDate.Value,
                datum_prestanka = datumPrestankaDatePicker.SelectedDate.Value,
                jezik_idjezik = 61,
                tema_idtema = 52
            };

            if (((ComboBoxItem)ulogaComboBox.SelectedItem).Content.ToString() == "Administrator")
            {
                noviZaposleni.uloga = "a";
            }
            else if (((ComboBoxItem)ulogaComboBox.SelectedItem).Content.ToString() == "Farmaceut")
            {
                noviZaposleni.uloga = "f";
            }

            
            bool uspesnoDodato = ZaposleniDB.DodajZaposlenog(noviZaposleni);

            if (uspesnoDodato)
            {
                MessageBox.Show("Zaposleni je uspješno dodat.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                ucitajZaposlene(); 
            }
            else
            {
                MessageBox.Show("Došlo je do greške pri dodavanju zaposlenog.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ocistiSvaPolja();
        }

        private void povratak_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (zaposleniDataGrid.SelectedItem is Zaposleni selektovaniZaposleni)
            {
                
                var rezultat = MessageBox.Show($"Da li ste sigurni da želite obrisati zaposlenog {selektovaniZaposleni.ime} {selektovaniZaposleni.prezime}?",
                                               "Potvrda brisanja",
                                               MessageBoxButton.YesNo,
                                               MessageBoxImage.Question);

                if (rezultat == MessageBoxResult.Yes)
                {
                    
                    bool uspesnoObrisano = ZaposleniDB.ObrisiZaposlenog(selektovaniZaposleni.idzaposleni);

                    if (uspesnoObrisano)
                    {
                        MessageBox.Show("Zaposleni je uspješno obrisan.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                        ucitajZaposlene(); 
                    }
                    else
                    {
                        MessageBox.Show("Došlo je do greške pri brisanju zaposlenog.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Niste odabrali zaposlenog za brisanje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            ucitajZaposlene();
        }
        private void ocistiSvaPolja()
        {
            jmbTextBox.Text = "";
            imeTextBox.Text = "";
            prezimeTextBox.Text = "";
            korisnickoImeTextBox.Text = "";
            lozinkaTextBox.Text = "";
            ulogaComboBox.SelectedIndex = -1;
            datumZaposlenjaDatePicker.SelectedDate = null;
            datumPrestankaDatePicker.SelectedDate = null;
        }

    }
}
