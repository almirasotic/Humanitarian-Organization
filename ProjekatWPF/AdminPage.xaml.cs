using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjekatWPF
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        public DB db
        {
            get; set;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string nacin;
        private Organizacija o;
        public Organizacija O
        {
            get { return o; }
            set
            {
                o = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Organizacija> organizacije
        {
            get; set;
        }


        public AdminPage()
        {
            db = new DB();
            O = new Organizacija();
            NovaOrganizacija = new Organizacija();
            //otvaranje prazne kolekcije
            organizacije = new ObservableCollection<Organizacija>();
            LoadData();
            InitializeComponent();
        }
        private void LoadData()
        {
            var dbOrganizacije = db.GetOrganizacija();
            organizacije.Clear();
            foreach (var art in dbOrganizacije)
            {
                organizacije.Add(art);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Organizacija selectedOrganization;
        public Organizacija SelectedOrganization
        {
            get { return selectedOrganization; }
            set
            {
                selectedOrganization = value;
                OnPropertyChanged(nameof(SelectedOrganization));
            }
        }
          private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedIndex>-1)
            {
                NovaOrganizacija.Ime = (dataGrid.SelectedItem as Organizacija).Ime;
                StaroIme = (dataGrid.SelectedItem as Organizacija).Ime;
                NovaOrganizacija.Donacija = (dataGrid.SelectedItem as Organizacija).Donacija;
            }
        }
        private Organizacija novaOrganizacija;

        public Organizacija NovaOrganizacija
        {
            get { return novaOrganizacija; }
            set { novaOrganizacija = value; OnPropertyChanged(); }
        }


        private void Unesi_Click(object sender, RoutedEventArgs e)
        {
            db.UnosOrganizacije(NovaOrganizacija);
            LoadData();
            NovaOrganizacija.Ime = string.Empty;
            NovaOrganizacija.Donacija = 0;
        }
        public string StaroIme;

        private void Izmeni_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex > -1) {
                db.IzmeniOrganizaciju(StaroIme,NovaOrganizacija);
                LoadData();

                }
        }
    }
}
