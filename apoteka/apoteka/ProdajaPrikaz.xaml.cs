using apoteka.baza;
using apoteka.model;
using System.Windows;

namespace apoteka
{
    public partial class ProdajaPrikaz : Window
    {
        Zaposleni zaposleni;
        public ProdajaPrikaz(Zaposleni zaposleni)
        {
            InitializeComponent();
            ucitajRacune();
            this.zaposleni = zaposleni;
            PromeniTemu();
            PromeniJezik();
        }

        private void PromeniJezik()
        {
           if(zaposleni.jezik_idjezik==61)
           {
                racuniTitleTextBlock.Text = "Racuni";
                racuniDataGrid.Columns[0].Header = "IdRacuna";
                racuniDataGrid.Columns[1].Header = "Datum";
                racuniDataGrid.Columns[2].Header = "Ukupan iznos";
                racuniDataGrid.Columns[3].Header = "IdZaposleni";
            }
           else
           {
                racuniTitleTextBlock.Text = "Bills";
                racuniDataGrid.Columns[0].Header = "Bills ID";
                racuniDataGrid.Columns[1].Header = "Date";
                racuniDataGrid.Columns[2].Header = "Total amount";
                racuniDataGrid.Columns[3].Header = "Employed ID";
            }
        }

        private void PromeniTemu()
        {
            App aplikacija = (App)Application.Current;
            aplikacija.PromijeniTemu(zaposleni.tema_idtema);
        }

        private void ucitajRacune()
        {
            var racuni = RacunDB.GetAllRacuni();
            racuniDataGrid.ItemsSource = racuni;
        }

        private void povratak_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
