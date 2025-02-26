using apoteka.baza;
using apoteka.model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace apoteka
{
    public partial class PodesavanjaPrikaz : Window
    {
        Zaposleni zaposleniPodesavanja;
        public PodesavanjaPrikaz(Zaposleni zaposleni)
        {
            this.zaposleniPodesavanja = zaposleni;
            InitializeComponent();
            UpdateToggleButton();
            UpdateToggle_Button();
            if (zaposleniPodesavanja.jezik_idjezik == 61)
            {
                PodesavanjaTitleTextBlock.Text = "Podesavanja";
                podesavanjaNalogaTextBlock.Text = "Podesavanja naloga:";
                novaLozinkaTextBlock.Text = "Nova lozinka:";
                potvrdaLozinkeTextBlock.Text = "Potvrda lozinke:";
                PromijeniTemuTextBlock.Text = "Promijeni temu:";
                PromijeniJezikTextBlock.Text = "Promijeni jezik:";
            }
            else
            {
                PodesavanjaTitleTextBlock.Text = "Settings";
                podesavanjaNalogaTextBlock.Text = "Account settings:";
                novaLozinkaTextBlock.Text = "New password:";
                potvrdaLozinkeTextBlock.Text = "Password confirmation:";
                PromijeniTemuTextBlock.Text = "Change theme:";
                PromijeniJezikTextBlock.Text = "Change language:";
            }

        }

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (novaLozinkaBox.Visibility == Visibility.Visible)
            {
                novaLozinkaBoxPassword.Text = novaLozinkaBox.Password;
                novaLozinkaBoxPassword.Visibility = Visibility.Visible;
                novaLozinkaBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                novaLozinkaBox.Password = novaLozinkaBoxPassword.Text;
                novaLozinkaBox.Visibility = Visibility.Visible;
                novaLozinkaBoxPassword.Visibility = Visibility.Collapsed;
            }
            UpdateToggleButton();
        }

        private void Toggle_PasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (potvrdaLozinkeBox.Visibility == Visibility.Visible)
            {
                
                potvrdaLozinkeBoxPassword.Text = potvrdaLozinkeBox.Password;
                potvrdaLozinkeBoxPassword.Visibility = Visibility.Visible;
                potvrdaLozinkeBox.Visibility = Visibility.Collapsed;
            }
            else
            {
               
                potvrdaLozinkeBox.Password = potvrdaLozinkeBoxPassword.Text;
                potvrdaLozinkeBox.Visibility = Visibility.Visible;
                potvrdaLozinkeBoxPassword.Visibility = Visibility.Collapsed;
            }
            UpdateToggle_Button();
            
        }

        private void UpdateToggle_Button()
        {
            
            if (potvrdaLozinkeBox.Visibility == Visibility.Visible)
            {
                toggleButton.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/slike/prikazi.png")) };
            }
            else
            {
                toggleButton.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/slike/sakri.png")) };
            }
        }

        private void UpdateToggleButton()
        {
            
            if (novaLozinkaBox.Visibility == Visibility.Visible)
            {
                toggleeButton.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/slike/prikazi.png")) };
            }
            else
            {
                toggleeButton.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/slike/sakri.png")) };
            }
        }

        private void promijeniLozinku_Click(object sender, RoutedEventArgs e)
        {
            string novaLozinka = novaLozinkaBox.Password;
            string potvrdaLozinka = potvrdaLozinkeBox.Password;

            if (string.IsNullOrEmpty(novaLozinka) || string.IsNullOrEmpty(potvrdaLozinka))
            {
                MessageBox.Show("Molimo unesite novu lozinku i potvrdu lozinke.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (novaLozinka != potvrdaLozinka)
            {
                MessageBox.Show("Lozinke se ne poklapaju. Molimo pokušajte ponovo.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ZaposleniDB zaposleni = new ZaposleniDB();
            bool uspesnoPromenjeno =zaposleni.azurirajLozinku(zaposleniPodesavanja.idzaposleni, novaLozinka);

            if (uspesnoPromenjeno)
            {
                MessageBox.Show("Lozinka je uspješno promenjena.", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Došlo je do greške pri promijeni lozinke. Pokušajte ponovo.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ocistiPolja();
        }

        private int? DohvatiIdTemeIzComboBox()
        {
            if (temaComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                if (int.TryParse(selectedItem.Tag.ToString(), out int temaId))
                {
                    return temaId;
                }
            }
            return null; 
        }

        private int? DohvatiIdJezikaIzComboBox()
        {
            if (jezikComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                if (int.TryParse(selectedItem.Tag.ToString(), out int jezikId))
                {
                    return jezikId;
                }
            }
            return null;
        }

        private void promijeniTemu_Click(object sender, RoutedEventArgs e)
        {

            int? idtema = zaposleniPodesavanja.tema_idtema;
            idtema=DohvatiIdTemeIzComboBox();
            if (idtema.HasValue)
            {
               
                ZaposleniDB db = new ZaposleniDB();
                bool uspjeh = db.AzurirajTemu(zaposleniPodesavanja.idzaposleni,idtema.Value);

                if (uspjeh)
                {
                    MessageBox.Show("Tema je uspješno promijenjena.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                    ((App)Application.Current).PromijeniTemu(idtema.Value);
                    zaposleniPodesavanja.tema_idtema = idtema.Value;
             
                }
                else
                {
                    MessageBox.Show("Došlo je do greške prilikom ažuriranja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Niste izabrali validnu temu.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void promijeniJezik_Click(object sender, RoutedEventArgs e)
        {
            int? idjezika = zaposleniPodesavanja.jezik_idjezik;
            idjezika = DohvatiIdJezikaIzComboBox();
            if (idjezika.HasValue)
            {
                ZaposleniDB db = new ZaposleniDB();
                bool uspjeh=db.AzurirajJezik(zaposleniPodesavanja.idzaposleni,idjezika.Value);
                if (uspjeh)
                {
                    MessageBox.Show("Jezik je uspješno promijenjen.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (idjezika == 61)
                    {
                        PodesavanjaTitleTextBlock.Text = "Podesavanja";
                        podesavanjaNalogaTextBlock.Text = "Podesavanja naloga:";
                        novaLozinkaTextBlock.Text = "Nova lozinka";
                        potvrdaLozinkeTextBlock.Text = "Potvrda lozinke:";
                        PromijeniTemuTextBlock.Text = "Promijeni temu:";
                        PromijeniJezikTextBlock.Text = "Promijeni jezik:";
                        zaposleniPodesavanja.jezik_idjezik = 61;
                    }
                    else
                    {
                        PodesavanjaTitleTextBlock.Text = "Settings";
                        podesavanjaNalogaTextBlock.Text = "Account settings:";
                        novaLozinkaTextBlock.Text = "New password";
                        potvrdaLozinkeTextBlock.Text = "Password confirmation:";
                        PromijeniTemuTextBlock.Text = "Change theme:";
                        PromijeniJezikTextBlock.Text = "Change language:";
                        zaposleniPodesavanja.jezik_idjezik = 62;
                    }
                    App.PromeniJezik();
                }
            }

        }



        private void povratak_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ocistiPolja()
        {
            novaLozinkaBox.Password = string.Empty;
            novaLozinkaBoxPassword.Text = string.Empty;
            potvrdaLozinkeBoxPassword.Text = string.Empty;
            potvrdaLozinkeBox.Password = string.Empty;
        }

    }
}
