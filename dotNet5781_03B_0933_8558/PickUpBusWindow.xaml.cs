using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dotNet5781_03B_0933_8558
{
    /// <summary>
    /// Interaction logic for PickUpBusWindow.xaml
    /// </summary>
    public partial class PickUpBusWindow : Window
    {
        public PickUpBusWindow()
        {
            InitializeComponent();
        }

        public PickUpBusWindow(object item)
        {
            InitializeComponent();
            pickUp.DataContext = item;
        }


        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                if (!double.TryParse(text.Text, out double km))
                    MessageBox.Show("Please enter a correct number of km!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (km > 1200)
                    MessageBox.Show("The bus is unable to travel more than 1200 km!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (km > bus.KMLeftToTravel)
                    MessageBox.Show("The bus doesn't have enough fuel for this ride!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (km > bus.KMtoNextTreat)
                    MessageBox.Show("The bus is unable to travel this distance, due to expected treatment!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    double KMH = Math.Round(MainWindow.rnd.NextDouble() * 30 + 20, 2);
                    bus.Travel(km, KMH);
                    MessageBox.Show("The average speed of the bus is:  " + KMH.ToString() + " Km / h", "Information:", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/.]$");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
