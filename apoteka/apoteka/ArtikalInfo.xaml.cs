using apoteka.baza;
using apoteka.model;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace apoteka
{
    public partial class ArtikalInfo : Window
    {
        private ArtikliPrikaz prikaz;
        Zaposleni zaposleniInfo;
        public ArtikalInfo(Artikli artikli,ArtikliPrikaz prikaz, Zaposleni zaposleniInfo)
        {
            InitializeComponent();
            this.prikaz = prikaz;
            if (artikli != null)
            {
                IdArtiklaTextBox.Text = artikli.idartikal == 0 ? string.Empty : artikli.idartikal.ToString();
                nazivTextBox.Text = artikli.naziv ?? string.Empty;
                opisTextBox.Text = artikli.opis ?? string.Empty;
                cijenaTextBox.Text = artikli.cijena > 0 ? artikli.cijena.ToString() : string.Empty;
                kolicinaTextBox.Text = artikli.kolicina >= 0 ? artikli.kolicina.ToString() : string.Empty;
                RokTrajanjaTextBox.Text = artikli.rok_trajanja == DateTime.MinValue ? string.Empty : artikli.rok_trajanja.ToString("dd.MM.yyyy");

                string folderPath = @"apoteka\apoteka\slike";
                string putanjaDoSlike = System.IO.Path.Combine(folderPath, $"{artikli.idartikal}.jpg");
                artikalImage.Source = new BitmapImage(new Uri(putanjaDoSlike, UriKind.Absolute));
            }
            else
            {
                MessageBox.Show("Artikal objekat nije dostupan.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.zaposleniInfo = zaposleniInfo;
            PromeniTemu();
            PromeniJezik();
        }

        private void PromeniTemu()
        {
            App aplikacija = (App)Application.Current;
            aplikacija.PromijeniTemu(zaposleniInfo.tema_idtema);
        }

        private void PromeniJezik()
        {
            if(zaposleniInfo.jezik_idjezik==61)
            {
                artikalInfoTextBlock.Text = "ArtikalInfo";
                idArtiklaTextBlock.Text = "IdArtikla:";
                nazivTextBlock.Text = "Naziv:";
                opisTextBlock.Text = "Opis:";
                cijenaTextBlock.Text = "Cijena:";
                kolicinaTextBlock.Text = "Kolicina:";
                rokTrajanjaTextBlock.Text = "Rok trajanja:";

            }
            else
            {
                artikalInfoTextBlock.Text = "ItemInfo";
                idArtiklaTextBlock.Text = "Item ID:";
                nazivTextBlock.Text = "Name:";
                opisTextBlock.Text = "Description:";
                cijenaTextBlock.Text = "Price:";
                kolicinaTextBlock.Text = "Quantity:";
                rokTrajanjaTextBlock.Text = "Expiration date:";
            }
        }

        private void povratak_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updateArtikal_Click(object sender, EventArgs e)
        {
            
            string noviNaziv = nazivTextBox.Text;
            string noviOpis = opisTextBox.Text;
            string novaCijena= cijenaTextBox.Text;
            string novaKolicina = kolicinaTextBox.Text;
            string rokTrajanjaTekst = RokTrajanjaTextBox.Text;

            
            if (string.IsNullOrWhiteSpace(noviNaziv) || string.IsNullOrWhiteSpace(noviOpis))
            {
                MessageBox.Show("Polja naziv ili opis ne mogu biti prazna.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            if (!decimal.TryParse(novaCijena, out decimal cijena) || cijena <= 0)
            {
                MessageBox.Show("Unesite ispravnu pozitivnu vrijednost za cijenu.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            if (!int.TryParse(novaKolicina, out int kolicina) || kolicina < 0)
            {
                MessageBox.Show("Unesite ispravnu nenegativnu vrijednost za količinu.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime rokTrajanja;


            if (string.IsNullOrWhiteSpace(rokTrajanjaTekst) || !DateTime.TryParseExact(rokTrajanjaTekst, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out rokTrajanja))
            {
                MessageBox.Show("Neispravan format datuma za rok trajanja. Unesite datum u formatu dd.MM.yyyy.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

           
            try
            {
                Artikli updatedArtikal = new Artikli
                {
                    idartikal = int.Parse(IdArtiklaTextBox.Text),
                    naziv = noviNaziv,
                    opis = noviOpis,
                    cijena = decimal.Parse(novaCijena),
                    kolicina=int.Parse(novaKolicina),
                    rok_trajanja=rokTrajanja,
                };

                
                bool uspeh = apoteka.baza.ArtikliDB.AzurirajArtikal(updatedArtikal);

                if (uspeh)
                {
                    MessageBox.Show("Podaci su uspešno ažurirani!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

                    
                    nazivTextBox.Text = updatedArtikal.naziv;
                    opisTextBox.Text = updatedArtikal.opis;
                    cijenaTextBox.Text = updatedArtikal.cijena.ToString();
                    kolicinaTextBox.Text = updatedArtikal.kolicina.ToString();
                    RokTrajanjaTextBox.Text = updatedArtikal.rok_trajanja.ToString("dd.MM.yyyy");
                }
                else
                {
                    MessageBox.Show("Došlo je do greške prilikom ažuriranja podataka u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom ažuriranja artikla: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            prikaz.ucitajArtikle();
        }
    }
}
