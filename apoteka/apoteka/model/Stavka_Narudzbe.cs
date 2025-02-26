using System;

namespace apoteka.model
{
    internal class Stavka_Narudzbe
    {
        public int idstavka_narudzbe { get; set; }
        public int kolicina { get; set; }
        public decimal? cijena { get; set; }
        public int narudzba_idnarudzba { get; set; }
        public int artikal_idartikal { get; set; }

        public Stavka_Narudzbe() { }

        public Stavka_Narudzbe(int idstavka_narudzbe, int kolicina, decimal? cijena, int narudzba_idnarudzba, int artikal_idartikal)
        {
            this.idstavka_narudzbe = idstavka_narudzbe;
            this.kolicina = kolicina;
            this.cijena = cijena;
            this.narudzba_idnarudzba = narudzba_idnarudzba;
            this.artikal_idartikal = artikal_idartikal;
        }

        public Stavka_Narudzbe(int kolicina, decimal? cijena, int narudzba_idnarudzba, int artikal_idartikal)
        {
            this.kolicina = kolicina;
            this.cijena = cijena;
            this.narudzba_idnarudzba = narudzba_idnarudzba;
            this.artikal_idartikal = artikal_idartikal;
        }

        public override bool Equals(object? obj)
        {
            return obj is Stavka_Narudzbe stavke_narudzbe &&
                   idstavka_narudzbe == stavke_narudzbe.idstavka_narudzbe;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(idstavka_narudzbe);
        }

        public override string ToString()
        {
            return $"Stavka_narudzbe: ID: {idstavka_narudzbe}, Kolicina:{kolicina}, Cijena: {cijena}, Narudzba_idNarudzba: {narudzba_idnarudzba}, Artikal_idArtikal: {artikal_idartikal}";
        }
    }
}
