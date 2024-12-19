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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        public Window1()
        {
            InitializeComponent();
            K = new Korisnik();
        }
        public DB db = new DB();
        private Korisnik korisnik;
        public ObservableCollection<Korisnik> Korisnici { get; set; }

        public Korisnik K
        {
            get { return korisnik; }
            set { korisnik = value; OnPropertyChanged(); }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(K.KorisnickoIme) || !string.IsNullOrWhiteSpace(K.KorisnickoIme))
            {
                K = db.Logovanje(K.KorisnickoIme);
                if (string.IsNullOrEmpty(K.KorisnickoIme))
                {
                    MessageBox.Show("Ne postoji korisnik sa takvim korisnickim imenom");
                    K.KorisnickoIme = string.Empty;
                    txtLozinka.Password = string.Empty;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(txtLozinka.Password) || !string.IsNullOrEmpty(txtLozinka.Password))
                    {
                        if (txtLozinka.Password.Trim() == K.Sifra)
                        {
                            if(K.Uloga == "User")
                            {
                                MainWindow mw = new MainWindow();
                                mw.Show();
                                this.Close();
                            }
                            else
                            {
                                AdminPage adminPage = new AdminPage();
                                adminPage.Show();
                                this.Close();
                            }
                            
                        }
                        else
                        {
                            MessageBox.Show("Pogresna lozinka");
                            txtLozinka.Password = string.Empty;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unesite lozinku");
                    }
                }
            }
            else
            {
                MessageBox.Show("Unesite korisnicko ime");
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(txtIme.Text==string.Empty 
                || txtLozinka.Password == string.Empty)
            {
                MessageBox.Show("Unesite podatke ");
                return;
            }
            K.Sifra = txtLozinka.Password.Trim(); 
            db.Registracija(K);
            txtLozinka.Password = string.Empty;
            K.KorisnickoIme = string.Empty;
            MessageBox.Show("Uspesna registracija korisnika");
        }

        private void Izlaz(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
