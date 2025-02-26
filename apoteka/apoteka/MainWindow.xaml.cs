using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using apoteka.baza;

namespace apoteka
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateToggleButton();
        }


        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Visibility == Visibility.Visible)
            {
                
                textBoxPassword.Text = passwordBox.Password;
                textBoxPassword.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
               
                passwordBox.Password = textBoxPassword.Text;
                passwordBox.Visibility = Visibility.Visible;
                textBoxPassword.Visibility = Visibility.Collapsed;
            }
            UpdateToggleButton();
        }

        private void UpdateToggleButton()
        {
            
            if (passwordBox.Visibility == Visibility.Visible)
            {
                toggleButton.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/slike/prikazi.png")) };
            }
            else
            {
                toggleButton.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/slike/sakri.png")) };
            }
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            
            UpdateToggleButton();

            
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                toggleButton.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/slike/prikazi")) };
                textBoxPassword.Visibility = Visibility.Collapsed; 
            }
            else
            {
                
                toggleButton.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/slike/sakri.png")) };
            }
        }

        private void textBoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (passwordBox.Visibility == Visibility.Collapsed)
            {
                passwordBox.Password = textBoxPassword.Text;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
           
            txtUsername.Text = string.Empty;

            
            passwordBox.Password = string.Empty;

           
            txtUsername.Focus();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); 
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
            string korisnickoIme = txtUsername.Text;
            string lozinka = passwordBox.Password;

            
            ZaposleniDB zaposleniDB = new ZaposleniDB();

            
            var (isAuthenticated,zaposleni) = zaposleniDB.Autentikacija(korisnickoIme, lozinka);

            if (isAuthenticated && zaposleni != null)
            {

                App aplikacija = (App)Application.Current;
                aplikacija.PromijeniTemu(zaposleni.tema_idtema);
                if (zaposleni.uloga == "a")
                {
                    
                    AdministratorWindow adminWindow = new AdministratorWindow(zaposleni);
                    adminWindow.Show();
                    
                }
                else if (zaposleni.uloga == "f")
                {
                    
                    FarmaceutWindow farmaceutWindow = new FarmaceutWindow(zaposleni);
                    farmaceutWindow.Show();
                    
                }
                else
                {
                    MessageBox.Show("Nepoznata uloga korisnika.","Greška",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            else
            {
                
                MessageBox.Show("Pogrešno korisničko ime ili lozinka.","Greška",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
