using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apoteka.model
{
    public class Artikli
    {
        public int idartikal {  get; set; }
        public string? naziv {  get; set; }
        public string? opis {  get; set; }

        public decimal? cijena { get; set; }
        public int kolicina { get; set; }
       
        public DateTime rok_trajanja { get; set; }

        public Artikli() { }

        public Artikli(int idartikal, string? naziv, string? opis, decimal? cijena, int kolicina, DateTime rok_trajanja)
        {
            this.idartikal = idartikal;
            this.naziv = naziv;
            this.opis = opis;
            this.cijena = cijena;
            this.kolicina = kolicina;
            this.rok_trajanja = rok_trajanja;
        }

        public Artikli(string? naziv, string? opis, decimal? cijena, int kolicina, DateTime rok_trajanja)
        {
            this.naziv = naziv;
            this.opis = opis;
            this.cijena = cijena;
            this.kolicina = kolicina;
            this.rok_trajanja = rok_trajanja;
        }

        public override bool Equals(object? obj)
        {
            return obj is Artikli artikli &&
                   idartikal == artikli.idartikal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(idartikal);
        }

        public override string ToString()
        {
            return $"Artikal: ID: {idartikal}, Naziv:{naziv} Opis: {opis}, Cijena: {cijena}, Kolicina: {kolicina}, Rok_trajanja: {rok_trajanja.ToShortDateString()}";
        }
    }
}
