using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apoteka.model
{
    class Dobavljac
    {
        public int iddobavljac { get; set; }
        public string? naziv { get; set; }
        public string? telefon { get; set; }
        public string? adresa { get; set; }
        public string? email { get; set; }

        public Dobavljac() { }

        public Dobavljac(int iddobavljac, string? naziv, string? telefon, string? adresa, string? email)
        {
            this.iddobavljac = iddobavljac;
            this.naziv = naziv;
            this.telefon = telefon;
            this.adresa = adresa;
            this.email = email;
        }

        public Dobavljac(string? naziv, string? telefon, string? adresa, string? email)
        {
            this.naziv = naziv;
            this.telefon = telefon;
            this.adresa = adresa;
            this.email = email;
        }

        public override bool Equals(object? obj)
        {
            return obj is Dobavljac dobavljac &&
                   iddobavljac == dobavljac.iddobavljac;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(iddobavljac);
        }

        public override string ToString()
        {
            return $"Dobavljač: ID: {iddobavljac}, Naziv:{naziv} Telefon: {telefon}, Adresa: {adresa}, Email: {email}";
        }
    }
}
