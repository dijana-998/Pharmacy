using apoteka.model;
using System;
using System.Windows;

namespace apoteka
{
    public partial class ZaposleniInfo : Window
    {
        private ZaposleniPrikaz prikaz;
        Zaposleni zap;
        public ZaposleniInfo(Zaposleni zaposleni,ZaposleniPrikaz prikaz, Zaposleni zap)
        {
            InitializeComponent();
            this.prikaz = prikaz;
            if (zaposleni != null)
            {
                IdZaposleniTextBox.Text = zaposleni.idzaposleni == 0 ? string.Empty : zaposleni.idzaposleni.ToString();
                JMBTextBox.Text = zaposleni.jmb ?? string.Empty;
                ImeTextBox.Text = zaposleni.ime ?? string.Empty;
                PrezimeTextBox.Text = zaposleni.prezime ?? string.Empty;
                KorisnickoImeTextBox.Text = zaposleni.korisnicko_ime ?? string.Empty;
                LozinkaTextBox.Text = zaposleni.lozinka ?? string.Empty;
                DatumZaposlenjaTextBox.Text = zaposleni.datum_zaposlenja == DateTime.MinValue ? string.Empty : zaposleni.datum_zaposlenja.ToString("dd.MM.yyyy");
                DatumPrestankaTextBox.Text = zaposleni.datum_prestanka == DateTime.MinValue ? string.Empty : zaposleni.datum_prestanka.ToString("dd.MM.yyyy");
            }
            else
            {
                MessageBox.Show("Zaposleni objekat nije dostupan.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.zap = zap;
            PromeniTemu();
            PromeniJezik();
        }

        private void PromeniTemu()
        {
            App aplikacija = (App)Application.Current;
            aplikacija.PromijeniTemu(zap.tema_idtema);
        }

        private void PromeniJezik()
        {
           if(zap.jezik_idjezik==61)
            {
                zaposleniInfoTitleTextBlock.Text = "ZaposleniInfo";
                idZaposlenogTextBlock.Text = "IdZaposlenog:";
                JMBTextBlock.Text = "JMB:";
                imeTextBlock.Text = "Ime:";
                prezimeTextBlock.Text = "Prezime:";
                korisnickoImeTextBlock.Text = "Korisnicko ime:";
                lozinkaTextBlock.Text = "Lozinka:";
                datumZaposlenjaTextBlock.Text = "Datum zaposlenja:";
                datumPrestankaTextBlock.Text = "Datum prestanka:";
            }
           else
            {
                zaposleniInfoTitleTextBlock.Text = "EmployeeInfo";
                idZaposlenogTextBlock.Text = "Employee ID:";
                JMBTextBlock.Text = "UIN:";
                imeTextBlock.Text = "Name:";
                prezimeTextBlock.Text = "Surname:";
                korisnickoImeTextBlock.Text = "Username:";
                lozinkaTextBlock.Text = "Password:";
                datumZaposlenjaTextBlock.Text = "Employment Date:";
                datumPrestankaTextBlock.Text = "Termination Date:";

            }
        }

        private void updateZaposleni_Click(object sender, EventArgs e)
        {
            
            string novoIme = ImeTextBox.Text;
            string novoPrezime = PrezimeTextBox.Text;
            string novoKorisnickoIme = KorisnickoImeTextBox.Text;
            string novaLozinka = LozinkaTextBox.Text;
            string datumZaposlenjaTekst = DatumZaposlenjaTextBox.Text;
            string datumPrestankaTekst = DatumPrestankaTextBox.Text;

            
            if (string.IsNullOrWhiteSpace(novoIme) || string.IsNullOrWhiteSpace(novoPrezime) ||
                string.IsNullOrWhiteSpace(novoKorisnickoIme) || string.IsNullOrWhiteSpace(novaLozinka))
            {
                MessageBox.Show("Polja ne mogu biti prazna.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime datumZaposlenja;
            DateTime datumPrestanka;

            
            if (string.IsNullOrWhiteSpace(datumZaposlenjaTekst) || !DateTime.TryParseExact(datumZaposlenjaTekst, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out datumZaposlenja))
            {
                MessageBox.Show("Neispravan format datuma zaposlenja. Unesite datum u formatu dd.MM.yyyy.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(datumPrestankaTekst) || !DateTime.TryParseExact(datumPrestankaTekst, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out datumPrestanka))
            {
                MessageBox.Show("Neispravan format datuma prestanka. Unesite datum u formatu dd.MM.yyyy.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            try
            {
                Zaposleni updatedZaposleni = new Zaposleni
                {
                    idzaposleni = int.Parse(IdZaposleniTextBox.Text),
                    jmb = JMBTextBox.Text,
                    ime = novoIme,
                    prezime = novoPrezime,
                    korisnicko_ime = novoKorisnickoIme,
                    lozinka = novaLozinka,
                    datum_zaposlenja = datumZaposlenja,
                    datum_prestanka = datumPrestanka
                };

                
                bool uspeh = apoteka.baza.ZaposleniDB.AzurirajZaposlenog(updatedZaposleni);

                if (uspeh)
                {
                    MessageBox.Show("Podaci su uspješno ažurirani!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

                    
                    ImeTextBox.Text = updatedZaposleni.ime;
                    PrezimeTextBox.Text = updatedZaposleni.prezime;
                    KorisnickoImeTextBox.Text = updatedZaposleni.korisnicko_ime;
                    LozinkaTextBox.Text = updatedZaposleni.lozinka;
                    DatumZaposlenjaTextBox.Text = updatedZaposleni.datum_zaposlenja.ToString("dd.MM.yyyy");
                    DatumPrestankaTextBox.Text = updatedZaposleni.datum_prestanka.ToString("dd.MM.yyyy");
                }
                else
                {
                    MessageBox.Show("Došlo je do greške prilikom ažuriranja podataka u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom ažuriranja zaposlenog: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            prikaz.ucitajZaposlene();
        }

        private void povratak_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
