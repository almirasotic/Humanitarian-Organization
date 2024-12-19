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
    /// Interaction logic for OdabirProzor.xaml
    /// </summary>
    public partial class OdabirProzor : Window,INotifyPropertyChanged
    {
        public DB db
        {
            get; set;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string nacin;
        private Ponuda o;
        private Racun ra = new Racun();
        public Ponuda O
        {
            get { return o; }
            set
            {
                o = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Ponuda> organizacije
        {
            get; set;
        }


        public OdabirProzor()
        {

            db = new DB();
            O = new Ponuda();
            //otvaranje prazne kolekcije
            organizacije = new ObservableCollection<Ponuda>();
            LoadData();
            InitializeComponent();

        }
        private void LoadData()
        {
            var dbOrganizacije = db.GetPonude();
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
        private void DonationButton_Click(object sender, RoutedEventArgs e)
        {
            if (nacin == "Gotovina")
            {
                if (dataGrid.SelectedIndex>-1 && float.TryParse(donationAmountTextBox.Text, out float donationAmount))
                {
                   
                    db.IzmeniDonaciju(o.ImeOrg, float.Parse(donationAmountTextBox.Text));
                    db.IzmeniPonudu(o.ImeOrg, float.Parse(donationAmountTextBox.Text));
                    LoadData();

                    donationAmountTextBox.Text = "";
                    ra.FirstMessageBoxText = $"Dodato {donationAmount:C} donacija za organizaciju '{o.ImeOrg}'";
                    ra.SecondMessageBoxText = $"Hvala sto hitno donirate";
                    ra.Show();
                    ra.ShowMessageBox();
                  
                }

                else
                {
                    MessageBox.Show("Nije odabrana organizacija ili neispravan unos za donaciju.");
                }
            }
            else if (nacin == "Kartica")
            {
                  if (dataGrid.SelectedIndex>-1 )
                {
                    CardPaymentPage cp = new CardPaymentPage(o.ImeOrg);
                    this.Close();
                    cp.Show();
                }
                else
                {
                    MessageBox.Show("Nije odabrana organizacija.");
                }
            }
            else
            {
                MessageBox.Show("Niste odabrali nacin placanja");
            }
        }
        private void CardRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            nacin = "Kartica";
        }





        private void ShowThankYouMessage()
        {
            MessageBox.Show("Hvala vam na donaciji!");
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();

            dataGrid.SelectionChanged += DataGrid_SelectionChanged;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedIndex>-1)
            {
                o.ImeOrg = (dataGrid.SelectedItem as Ponuda).ImeOrg;
                o.Tekst = (dataGrid.SelectedItem as Ponuda).Tekst;
                o.PotrebnaKol = (dataGrid.SelectedItem as Ponuda).PotrebnaKol;

            }
        }

        private void cashRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            nacin = "Gotovina";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OdabirProzor op = new OdabirProzor();
            op.Show();
        }
    }
}
