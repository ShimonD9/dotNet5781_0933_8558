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
    /// Interaction logic for BusStopDetailsWindowWindow.xaml
    /// </summary>
    public partial class BusStopDetailsWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");
        BO.BusStop busStop;
        int oldBusStopKey;

        public BusStopDetailsWindow()
        {
            InitializeComponent();
        }

        // A second builder, to get the item selected in the list box
        public BusStopDetailsWindow(object item)
        {
            InitializeComponent();
            BusStopDet.DataContext = item;
            busStop = item as BO.BusStop;
            oldBusStopKey = busStop.BusStopKey;
        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Double.TryParse(tbLatitude.GetLineText(0), out double lati) || !Double.TryParse(tbLongitude.GetLineText(0), out double longi))
                {
                    MessageBox.Show("You didn't fill correctly all the required information", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (double.Parse(tbLatitude.Text) > 33.3 || double.Parse(tbLatitude.Text) < 31 || double.Parse(tbLongitude.Text) < 34.3 || double.Parse(tbLongitude.Text) > 35.5)
                {
                    MessageBox.Show("The bus company is in Israel, the coordinates should be in range!", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    busStop.Sunshade = (bool)cbSunshade.IsChecked;
                    busStop.DigitalPanel = (bool)cbDigitalPanel.IsChecked;
                    busStop.BusStopKey = int.Parse(tbStopKey.GetLineText(0));
                    busStop.BusStopName = tbStopName.GetLineText(0);
                    busStop.BusStopAddress = tbAddress.GetLineText(0);
                    busStop.Latitude = lati;
                    busStop.Longitude = longi;
                    bl.UpdateBusStop(oldBusStopKey, busStop);   // Inserts the new bus to the beginning of the list                 
                    this.Close();
                }
            }
            catch (BO.ExceptionBLBadLicense)
            {
                MessageBox.Show("The updated bus stop code you entered already exists in the company!", "Cannot add the bus stop", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this bus stop?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bl.DeleteBusStop(busStop.BusStopKey);
                    this.Close(); // Closes the window
                }
                catch (BO.ExceptionBLBadLicense)
                {
                    MessageBox.Show("The bus license doesn't exist or the bus is inactive!", "Cannot delete the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch
                {
                    string busLines = string.Join(", ", from lineBus in busStop.LinesStopHere select lineBus.BusLineNumber);
                    MessageBox.Show("This bus stop serves the next bus lines: \n" + busLines + ".\nYou must remove the bus station from the bus lines details window, before deleting the bus stop itself.", "Unable to delte the bus stop!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
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
    }
}
