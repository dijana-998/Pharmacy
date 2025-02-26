using System;

namespace apoteka.model
{
    internal class Narudzba
    {
        public int idnarudzba {  get; set; }
        public DateTime datum {  get; set; }
        public decimal? ukupna_cijena { get; set; }
        public int dobavljac_iddobavljac {  get; set; }

        public Narudzba() { }
        public Narudzba(int idnarudzba, DateTime datum, decimal? ukupna_cijena, int dobavljac_iddobavljac)
        {
            this.idnarudzba = idnarudzba;
            this.datum = datum;
            this.ukupna_cijena = ukupna_cijena;
            this.dobavljac_iddobavljac = dobavljac_iddobavljac;
        }

        public Narudzba(DateTime datum, decimal? ukupna_cijena, int dobavljac_iddobavljac)
        {
            this.datum = datum;
            this.ukupna_cijena = ukupna_cijena;
            this.dobavljac_iddobavljac = dobavljac_iddobavljac;
        }

        public override bool Equals(object? obj)
        {
            return obj is Narudzba narudzbe &&
                   idnarudzba == narudzbe.idnarudzba;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(idnarudzba);
        }

        public override string ToString()
        {
            return $"Naruzdba: ID: {idnarudzba}, Datum:{datum.ToShortDateString()} , Ukupna_cijena: {ukupna_cijena}, Dobavljac_idDobavljac: {dobavljac_iddobavljac}";
        }

    }
}
