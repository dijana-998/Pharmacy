using System;


namespace apoteka.model
{
    public class Zaposleni
    {
        public int idzaposleni {  get; set; }
        public string? jmb {  get; set; }
        public string? ime {  get; set; }
        public string? prezime { get; set; }
        public string? korisnicko_ime { get; set; }
        public string? lozinka { get; set; }
        public string? uloga { get; set; }
        public DateTime datum_zaposlenja { get; set; }
        public DateTime datum_prestanka { get; set; }
        public int? jezik_idjezik {  get; set; }
        public int? tema_idtema { get; set; }


        public Zaposleni()
        {

        }

        public Zaposleni(int idzaposleni, string jmb, string ime, string prezime, string korisnicko_ime, string lozinka, string uloga, DateTime datum_zaposlenja, DateTime datum_prestanka, int jezik_idjezik, int tema_idtema)
        {
            this.idzaposleni = idzaposleni;
            this.jmb = jmb;
            this.ime = ime;
            this.prezime = prezime;
            this.korisnicko_ime = korisnicko_ime;
            this.lozinka = lozinka;
            this.uloga = uloga;
            this.datum_zaposlenja = datum_zaposlenja;
            this.datum_prestanka = datum_prestanka;
            this.jezik_idjezik = jezik_idjezik;
            this.tema_idtema = tema_idtema;
        }

        public Zaposleni(string jmb, string ime, string prezime, string korisnicko_ime, string lozinka, string uloga, DateTime datum_zaposlenja, DateTime datum_prestanka, int jezik_idjezik, int tema_idtema)
        {
            this.jmb = jmb;
            this.ime = ime;
            this.prezime = prezime;
            this.korisnicko_ime = korisnicko_ime;
            this.lozinka = lozinka;
            this.uloga = uloga;
            this.datum_zaposlenja = datum_zaposlenja;
            this.datum_prestanka = datum_prestanka;
            this.jezik_idjezik = jezik_idjezik;
            this.tema_idtema = tema_idtema;
        }

        public override bool Equals(object? obj)
        {
            return obj is Zaposleni zaposleni &&
                   idzaposleni == zaposleni.idzaposleni;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(idzaposleni);
        }

        public override string ToString()
        {
            return $"Zaposleni: ID: {idzaposleni}, Ime: {ime}, Prezime: {prezime}, Uloga: {uloga}, Datum Zaposlenja: {datum_zaposlenja.ToShortDateString()}, Datum Prestanka: {datum_prestanka.ToShortDateString()}, Jezik: {jezik_idjezik}, Tema: {tema_idtema}";
        }
    }
}
