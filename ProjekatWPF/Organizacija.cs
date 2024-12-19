using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatWPF
{
    public class Organizacija:INotifyPropertyChanged
    {
		private string	ime;

		public  string Ime
		{
			get { return ime; }
			set { ime = value; OnPropertyChanged(); }
		}
        private float donacija;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public float Donacija
        {
            get { return donacija; }
            set { donacija = value; OnPropertyChanged(); }
        }
        public Organizacija()
        {
            this.Ime = ime;
            this.Donacija = donacija;
        }

    }
}
