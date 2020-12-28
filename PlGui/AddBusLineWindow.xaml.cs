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
            cbArea.ItemsSource = Enum.GetValues(typeof(PO.Enums.AREA));
            cbSecondBusStop.IsEnabled = false;
            cbFirstBusStop.ItemsSource = bl.GetAllBusStops();
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/.]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationTextBoxNoDots(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cbFirstBusSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbSecondBusStop.IsEnabled = true;
            // Not completed (how skipWhile works??)
            cbSecondBusStop.ItemsSource = bl.GetAllBusStops().SkipWhile(busStop => busStop.BusStopKey == (cbFirstBusStop.SelectedItem as BO.BusStop).BusStopKey);
        }
    }
}
