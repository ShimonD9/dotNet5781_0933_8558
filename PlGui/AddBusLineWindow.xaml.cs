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
using BLApi;
using BO;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for AddBusLineWindow.xaml
    /// </summary>
    public partial class AddBusLineWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");


        public AddBusLineWindow()
        {
            InitializeComponent();
            cbArea.ItemsSource = Enum.GetValues(typeof(BO.Enums.AREA));
            cbLastBusStop.IsEnabled = false;
            cbFirstBusStop.ItemsSource = bl.GetAllBusStops().OrderBy(busStop => busStop.BusStopKey);
        }


        /// <summary>
        /// The adding bus button method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        // Using regex to unable wrongs inputs in the text box:

        private void NumberValidationTextBoxColon(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/:]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationTextBoxDots(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/:]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationTextBoxNoDots(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cbFirstBusStopSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbLastBusStop.IsEnabled = true;
            if (cbFirstBusStop.SelectedItem != null) // In case it will be changed to null because of the second combo box method
                cbLastBusStop.ItemsSource = bl.GetAllBusStops()
                    .Where(busStop => busStop.BusStopKey != (cbFirstBusStop.SelectedItem as BO.BusStop).BusStopKey)
                    .OrderBy(busStop => busStop.BusStopKey);
        }

        private void cbLastBusStopSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
