using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BLApi;
using BO;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    partial class MainWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");

        public MainWindow()
        {
            InitializeComponent();

        }



        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            tbPasswordShow.Text = pbPassword.Password;
            tbPasswordShow.Visibility = Visibility.Visible;
            pbPassword.Visibility = Visibility.Collapsed;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            tbPasswordShow.Visibility = Visibility.Collapsed;
            pbPassword.Visibility = Visibility.Visible;
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            spRepeatPassword.Visibility = Visibility.Visible;
            bSignIn.IsEnabled = false;
        }

        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<AdminDisplayWindow>().Any()) // To prevent the openning of another same window
            {
                AdminDisplayWindow adminDisplayWindow = new AdminDisplayWindow(); // Creates the new window, and then shows it
                adminDisplayWindow.ShowDialog();
            }
        }
    }
}
