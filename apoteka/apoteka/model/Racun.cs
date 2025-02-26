using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apoteka.model
{
    internal class Racun
    {
        public int idracun { get; set; }
        public DateTime datum { get; set; }
        public decimal? ukupan_iznos { get; set; }
        public int? zaposleni_idzaposleni { get; set; }

        public Racun() { }

        public Racun(int idracun, DateTime datum, decimal? ukupan_iznos, int? zaposleni_idzaposleni)
        {
            this.idracun = idracun;
            this.datum = datum;
            this.ukupan_iznos = ukupan_iznos;
            this.zaposleni_idzaposleni = zaposleni_idzaposleni;
        }

        public Racun(DateTime datum, decimal? ukupan_iznos, int? zaposleni_idzaposleni)
        {
            this.datum = datum;
            this.ukupan_iznos = ukupan_iznos;
            this.zaposleni_idzaposleni = zaposleni_idzaposleni;
        }

        public override bool Equals(object? obj)
        {
            return obj is Racun racun &&
                   idracun == racun.idracun;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(idracun);
        }

        public override string ToString()
        {
            return $"Racun: ID: {idracun}, Datum:{datum.ToShortDateString()} , Ukupan_iznos: {ukupan_iznos}, Zaposleni: {zaposleni_idzaposleni}";
        }

    }
}
