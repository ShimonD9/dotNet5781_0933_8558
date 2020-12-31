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
using BO;
using BLApi;


namespace PlGui
{


    /// <summary>
    /// Interaction logic for AddBusStopWindow.xaml
    /// </summary>
    public partial class AddBusStopWindow : Window
    {
        BO.BusStop newBusStop;
        IBL myBL;

        public AddBusStopWindow()
        {
            InitializeComponent();
            myBL = BLFactory.GetBL("1");
            newBusStop = new BO.BusStop();
        }


        /// <summary>
        /// The adding bus button method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Checks if the inputs are correct, and pops an appropriate message if not:
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
                    newBusStop.BusStopKey = int.Parse(tbBusStopCode.GetLineText(0));
                    newBusStop.BusStopAddress = tbBusStopAddress.GetLineText(0);
                    newBusStop.BusStopName = tbBusStopName.GetLineText(0);
                    newBusStop.Latitude = double.Parse(tbLatitude.GetLineText(0));
                    newBusStop.Longitude = double.Parse(tbLongitude.GetLineText(0));
                    newBusStop.Sunshade = cbSunshade.IsEnabled;
                    newBusStop.DigitalPanel = cbDigitalPanel.IsEnabled;
                    newBusStop.ObjectActive = true;
                    myBL.AddBusStop(newBusStop);   // Inserts the new bus to the beginning of the list                 
                    this.Close();
                }
            }
            catch (BO.ExceptionBLBadLicense)
            {
                MessageBox.Show("The bus stop code you entered already exists in the company!", "Cannot add the bus stop", MessageBoxButton.OK, MessageBoxImage.Warning);
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

