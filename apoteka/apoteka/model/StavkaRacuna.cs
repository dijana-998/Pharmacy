using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apoteka.model
{
    internal class StavkaRacuna
    {
        public int idstavka_racuna { get; set; }
        public int kolicina { get; set; }
        public decimal? cijena { get; set; }
        public int racun_idracun { get; set; }
        public int artikal_idartikal { get; set; }

        public StavkaRacuna() { }

        public StavkaRacuna(int idstavka_racuna, int kolicina, decimal? cijena, int racun_idracun, int artikal_idartikal)
        {
            this.idstavka_racuna = idstavka_racuna;
            this.kolicina = kolicina;
            this.cijena = cijena;
            this.racun_idracun = racun_idracun;
            this.artikal_idartikal = artikal_idartikal;
        }

        public StavkaRacuna(int kolicina, decimal? cijena, int racun_idracun, int artikal_idartikal)
        {
            this.kolicina = kolicina;
            this.cijena = cijena;
            this.racun_idracun = racun_idracun;
            this.artikal_idartikal = artikal_idartikal;
        }

        public override bool Equals(object? obj)
        {
            return obj is StavkaRacuna stavke_racuna &&
                   idstavka_racuna == stavke_racuna.idstavka_racuna;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(idstavka_racuna);
        }

        public override string ToString()
        {
            return $"StavkaRacuna: ID: {idstavka_racuna}, Kolicina:{kolicina}, Cijena: {cijena}, Racun_idRacun: {racun_idracun}, Artikal_idArtikal: {artikal_idartikal}";
        }
    }
}
