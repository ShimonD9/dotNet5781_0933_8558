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

        IBL bl = BLFactory.GetBL("1"); // Calls and stores the instance of the bl interface
        BO.BusStop newBusStop = new BO.BusStop(); // Creates a new BO.BusStop to be added

        /// <summary>
        /// Default window ctor
        /// </summary>
        public AddBusStopWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// The adding bus button method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                // Checks if the user filled the fields, and pops an appropriate message if not (no sending to bl for this, because the input checked as string by tryParse)
                if (tbBusStopCode.GetLineText(0) == "" || tbBusStopAddress.GetLineText(0) == "" || tbBusStopName.GetLineText(0) == "" ||                                  
                    !Double.TryParse(tbLatitude.GetLineText(0), out double lati) || !Double.TryParse(tbLongitude.GetLineText(0), out double longi) || int.Parse(tbBusStopCode.GetLineText(0)) == 0)
                {
                    MessageBox.Show("You didn't fill correctly all the required information", "Cannot add the bus stop", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Initializes the bus stop:
                    newBusStop.BusStopKey = int.Parse(tbBusStopCode.GetLineText(0));
                    newBusStop.BusStopAddress = tbBusStopAddress.GetLineText(0);
                    newBusStop.BusStopName = tbBusStopName.GetLineText(0);
                    newBusStop.Latitude = lati;
                    newBusStop.Longitude = longi;
                    newBusStop.Sunshade = (bool)cbSunshade.IsChecked;
                    newBusStop.DigitalPanel = (bool)cbDigitalPanel.IsChecked;
                    
                    bl.AddBusStop(newBusStop);   // Inserts the new bus to the beginning of the list                 
                    this.Close();
                }
            }

            catch (BO.ExceptionBL_KeyAlreadyExist) // In case the bus stop already exists in the company
            {
                MessageBox.Show("The bus stop code you entered already exists in the company!", "Cannot add the bus stop", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            catch (BO.ExceptionBL_Incorrect_coordinates) // For wrong coordinates
            {
                MessageBox.Show("The bus company is in Israel, the coordinates should be in range!", "Cannot add the bus stop", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // Using regex to unable wrongs inputs in the text box:

        /// <summary>
        /// Preview keyboard input to numbers with dots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/.]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Preview keyboard input to numbers only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBoxNoDots(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

