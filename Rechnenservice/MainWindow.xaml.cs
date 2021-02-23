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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Threading;
using System.Text.RegularExpressions;

namespace Rechnenservice
{
    public partial class MainWindow : Window
    {
        myDialog dlg;
        IPAddress host;
        Socket s;
        Thread thread_1;
        bool connectStatus;
        bool rechne;
        string data;
        byte[] bytes;
        string serverErgebnis;

        public MainWindow()
        {
            InitializeComponent();

            disconnect.IsEnabled = false;
            tbOperand1.IsReadOnly = true;
            tbOperand2.IsReadOnly = true;
            tbOperation.IsReadOnly = true;
        }

        private void ThreadFunction()
        {
            data = string.Empty;

            while (connectStatus == true)
            {
                try
                {
                    if (rechne == true)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                        {
                            data = tbOperand1.Text + "~" + tbOperation.Text + "~" + tbOperand2.Text;
                        }));

                        s.Send(Encoding.ASCII.GetBytes(data));

                        rechne = false;

                        //if (s.Available > 0)
                        //{
                            bytes = new byte[128];

                            s.Receive(bytes);

                            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                            {
                                serverErgebnis = Encoding.ASCII.GetString(bytes);

                                lbErgebnis.Content = serverErgebnis;
                            }));
                        //}
                    }
                }
                catch { }
            }
        }

        private void connect_Click(object sender, RoutedEventArgs e)
        {
            dlg = new myDialog();
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                disconnect.IsEnabled = true;
                connect.IsEnabled = false;              

                host = IPAddress.Parse(dlg.ipAddress);
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    s.Connect(host, dlg.port);
                }
                catch { }

                if (s.Connected)
                {
                    connectStatus = true;
                    disconnect.IsEnabled = true;
                    connect.IsEnabled = false;
                    tbOperand1.IsReadOnly = false;
                    tbOperand2.IsReadOnly = false;
                    tbOperation.IsReadOnly = false;
                    lbStatus.Content = "Verbunden";

                    thread_1 = new Thread(ThreadFunction);
                    thread_1.Start();
                }               
            }
        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            connectStatus = false;
            s.Close();
            connect.IsEnabled = true;
            disconnect.IsEnabled = false;
            lbStatus.Content = "Nicht verbunden";
        }

        private void btnRechne_Click(object sender, RoutedEventArgs e)
        {
            if(!Regex.IsMatch(tbOperand1.Text, "[1234567890,]") || !Regex.IsMatch(tbOperand2.Text, "[1234567890,]"))
            {
                MessageBox.Show("Es dürfen nur zahlen und kommas verwendet werden", "Eingabefehler]", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (!Regex.IsMatch(tbOperation.Text, "[+\\-*/]"))
            {
                MessageBox.Show("Folgende Zeichen dürfen nur verwendet werden: [+ - * /]", "Eingabefehler]", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            rechne = true;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (thread_1 != null)
                {
                    connectStatus = false;
                    thread_1.Abort();
                    s.Close();                   
                }
                Environment.Exit(0);
            }
            catch { }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (thread_1 != null)
                {
                    connectStatus = false;
                    thread_1.Abort();
                    s.Close();
                }                
                Environment.Exit(0);
            }
            catch { }
        }

        private void tbOperation_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void tbOperand1_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void tbOperand2_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }
    }
}
