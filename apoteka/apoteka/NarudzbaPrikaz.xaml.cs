using apoteka.baza;
using apoteka.model;
using System.Windows;

namespace apoteka
{
    public partial class NarudzbaPrikaz : Window
    {
        Zaposleni zaposleniNarudzba;
        public NarudzbaPrikaz(Zaposleni zaposleni)
        {
            InitializeComponent();
            ucitajNarudzbe();
            this.zaposleniNarudzba = zaposleni;
            PromeniTemu();
            PromeniJezik();
        }
        private void PromeniTemu()
        {
            App aplikacija = (App)Application.Current;
            aplikacija.PromijeniTemu(zaposleniNarudzba.tema_idtema);
        }

        private void PromeniJezik()
        {
            if(zaposleniNarudzba.jezik_idjezik==61)
            {
                narudzbaTitleTextBlock.Text = "Narudzba";
                narudzbaDataGrid.Columns[0].Header = "IdNarudzba";
                narudzbaDataGrid.Columns[1].Header = "Datum";
                narudzbaDataGrid.Columns[2].Header = "Ukupna cijena";
                narudzbaDataGrid.Columns[3].Header = "IdDobavljaca";
            }
            else
            {
                narudzbaTitleTextBlock.Text = "Order";
                narudzbaDataGrid.Columns[0].Header = "Order ID";
                narudzbaDataGrid.Columns[1].Header = "Date";
                narudzbaDataGrid.Columns[2].Header = "Total price";
                narudzbaDataGrid.Columns[3].Header = "Supplier ID";
            }
        }

        private void ucitajNarudzbe()
        {
            var narudzbe=NarudzbaDB.GetAllNarudzbe();
            narudzbaDataGrid.ItemsSource= narudzbe;
        }

        private void povratak_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
