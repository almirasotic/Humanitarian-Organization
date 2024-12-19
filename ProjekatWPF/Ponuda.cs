using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatWPF
{
    public class Ponuda : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string imeOrg;

        public string  ImeOrg
        {
            get { return imeOrg; }
            set { imeOrg = value; OnPropertyChanged(); }
        }
        private float potrebnaKol;

        public float PotrebnaKol
        {
            get { return potrebnaKol; }
            set { potrebnaKol = value; OnPropertyChanged(); }
        }
        private string tekst;

        public string Tekst
        {
            get { return tekst; }
            set { tekst = value; OnPropertyChanged(); }
        }



    }
}
