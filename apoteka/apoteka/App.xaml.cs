using apoteka.model;
using System;
using System.Windows;
namespace apoteka
{
  
    public partial class App : Application
    {

        public int? TrenutniTemaId { get; set; } = 52;
        public static event Action? JezikPromenjen;

        public static void PromeniJezik()
        {
            JezikPromenjen?.Invoke();
        }

        public void PromijeniTemu(int? temaId)
        {
            ResourceDictionary novaTema = new ResourceDictionary();
            TrenutniTemaId = temaId;

            switch (temaId)
            {
                case 51: // Tamna
                    novaTema = new ResourceDictionary { Source = new Uri("Teme/TamnaTema.xaml", UriKind.Relative) };
                    break;
                case 52: // Svijetla
                    novaTema = new ResourceDictionary { Source = new Uri("Teme/SvijetlaTema.xaml", UriKind.Relative) };
                    break;
                case 53: // Siva
                    novaTema = new ResourceDictionary { Source = new Uri("Teme/SivaTema.xaml", UriKind.Relative) };
                    break;
                default: // Default Svijetla
                    novaTema = new ResourceDictionary { Source = new Uri("Teme/SvijetlaTema.xaml", UriKind.Relative) };
                    break;
            }

            if (novaTema == null)
            {
                MessageBox.Show("Tema nije pronađena. Proverite postavke tema.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            Current.Resources.MergedDictionaries.Clear();
            Current.Resources.MergedDictionaries.Add(novaTema);
        }

    }
}
