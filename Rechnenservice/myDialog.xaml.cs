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

namespace Rechnenservice
{
    /// <summary>
    /// Interaktionslogik für myDialog.xaml
    /// </summary>
    public partial class myDialog : Window
    {
        public double ipFeld1 { get; set; }
        public double ipFeld2 { get; set; }
        public double ipFeld3 { get; set; }
        public double ipFeld4 { get; set; }
        public int port { get; set; }
        public string ipAddress { get; set; }

        public myDialog()
        {
            InitializeComponent();

            tbPort.IsReadOnly = true;
            tbPort.Background = Brushes.LightGray;

            //Maximale Anzahl an Zeichen pro Feld festlegen
            tbIp1.MaxLength = 3;
            tbIp2.MaxLength = 3;
            tbIp3.MaxLength = 3;
            tbIp4.MaxLength = 3;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ipFeld1 = Convert.ToDouble(tbIp1.Text);
                ipFeld2 = Convert.ToDouble(tbIp2.Text);
                ipFeld3 = Convert.ToDouble(tbIp3.Text);
                ipFeld4 = Convert.ToDouble(tbIp4.Text);
                port = Convert.ToInt32(tbPort.Text);

                //mw = new MainWindow();

                ipAddress = tbIp1.Text + "." + tbIp2.Text + "." + tbIp3.Text + "." + tbIp4.Text;

                if ((ipFeld1 < 0 || ipFeld1 > 255) || (ipFeld2 < 0 || ipFeld2 > 255) || (ipFeld3 < 0 || ipFeld3 > 255) || (ipFeld4 < 0 || ipFeld4 > 255))
                {
                    throw new Exception("Ungültige IP-Adresse");
                }
                else
                {
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message, "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbIp1_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void tbIp2_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void tbIp3_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void tbIp4_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }
    }
}
