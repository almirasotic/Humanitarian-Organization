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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjekatWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public DB db {
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
        private Racun ra = new Racun();


        public MainWindow()
        {
            
            db= new DB();
            O= new Organizacija();
            //otvaranje prazne kolekcije
            organizacije=new ObservableCollection<Organizacija>();
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
        private void DonationButton_Click(object sender, RoutedEventArgs e)
        {
            if (nacin == "Gotovina")
            {
                if (dataGrid.SelectedItem is Organizacija selectedOrganization && float.TryParse(donationAmountTextBox.Text, out float donationAmount))
                {
                    if (selectedOrganization.Donacija == 0)
                    {
                        selectedOrganization.Donacija = donationAmount;
                    }
                    else
                    {
                        selectedOrganization.Donacija += donationAmount;
                    }
                    db.IzmeniDonaciju((dataGrid.SelectedItem as Organizacija).Ime, float.Parse(donationAmountTextBox.Text));
                    LoadData();
                   
                    donationAmountTextBox.Text = "";
                  
                    ra.FirstMessageBoxText = $"Dodato {donationAmount:C} donacija za organizaciju '{selectedOrganization.Ime}'.\nTrenutno stanje donacija: {selectedOrganization.Donacija:C}";
                    ra.SecondMessageBoxText = $"Hvala Vam sto pomazete nasoj organizaciji sa iznosom od: {donationAmount:C}";
                    ra.Show();
                    ra.ShowMessageBox();
                }
                
                else
                {
                    MessageBox.Show("Nije odabrana organizacija ili neispravan unos za donaciju.");
                }
            }
            else if(nacin =="Kartica")
            {
                if (dataGrid.SelectedItem is Organizacija selectedOrganization)
                {
                    CardPaymentPage cp = new CardPaymentPage(selectedOrganization.Ime);
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
            if (dataGrid.SelectedItem is Organizacija selectedOrganization)
            {
                donationAmountTextBox.Text = "";
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
