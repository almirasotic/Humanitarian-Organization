using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for CardPaymentPage.xaml
    /// </summary>
    public partial class CardPaymentPage : Window
    {
        public DB db=new DB();
        //Organizacija o = new Organizacija();
        public CardPaymentPage()
        {
            InitializeComponent();
        }
        public string OrganizacijaIme { get; set; }
        public CardPaymentPage(string organizacijaIme) : this()
        {
            OrganizacijaIme = organizacijaIme;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Kartica k = db.UcitajKarticu(expirationMonthTextBox.Text.Trim() + expirationYearTextBox.Text.Trim());
            if(k.BrRacuna != expirationMonthTextBox.Text.Trim() + expirationYearTextBox.Text.Trim())
            {
                MessageBox.Show("Netacan br racuna");
                return;
            }
            if (k.KolicinaNovca >= int.Parse(cvvTextBox.Text)) {
                db.Naplati(expirationMonthTextBox.Text.Trim() + expirationYearTextBox.Text.Trim(), float.Parse(cvvTextBox.Text), OrganizacijaIme);
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Nema dovoljno novca na kartici");
            }
        }
    }
}
