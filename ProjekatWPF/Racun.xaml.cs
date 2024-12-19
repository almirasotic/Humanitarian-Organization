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
    /// Interaction logic for Racun.xaml
    /// </summary>
    public partial class Racun : Window
    {
        public string FirstMessageBoxText { get; set; }
        public string SecondMessageBoxText { get; set; }
        public Racun()
        {
            InitializeComponent();
        }
        public void ShowMessageBox()
        {
            firstTextBox.Text = FirstMessageBoxText;
            secondTextBox.Text = SecondMessageBoxText;
        }
    }
}
