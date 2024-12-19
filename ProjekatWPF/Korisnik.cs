using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatWPF
{
    public class Korisnik
    {
        private String korisnickoIme;
        private String sifra;
        private String uloga;
        public String KorisnickoIme
        {
            get { return korisnickoIme; }
            set { korisnickoIme = value; }
        }
        public String Sifra
        {
            get { return sifra; }
            set { sifra = value; }
        }
        private string brracuna;

        public string BrRacuna
        {
            get { return brracuna; }
            set { brracuna = value; }
        }

        public String Uloga
        {
            get { return uloga; }
            set { uloga = value; }
        }



        public Korisnik()
        {

        }
    }
}
