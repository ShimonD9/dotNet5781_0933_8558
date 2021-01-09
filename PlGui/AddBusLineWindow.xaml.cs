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
        BO.BusLine newBusLine = new BO.BusLine();
        BO.BusLineStation newStationA = new BusLineStation();
        BO.BusLineStation newStationB = new BusLineStation();

        int runningNumber;

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
            try
            {
                double kmToNext = 0;
                TimeSpan timeToNext = new TimeSpan(0, 0, 0);
                if (tbKmToNext.Visibility == Visibility.Visible && !Double.TryParse(tbKmToNext.GetLineText(0), out kmToNext) || tbTimeToNext.Visibility == Visibility.Visible && !TimeSpan.TryParse(tbTimeToNext.GetLineText(0), out timeToNext)
                    || !TimeSpan.TryParse(tbStartTime.GetLineText(0), out TimeSpan startTime) || !TimeSpan.TryParse(tbEndTime.GetLineText(0), out TimeSpan endTime))
                {
                    MessageBox.Show("You didn't fill correctly all the required information", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Bus Line addition
                    newBusLine.BusLineNumber = int.Parse(tbLineNumber.GetLineText(0));
                    newBusLine.Area = (Enums.AREA)cbArea.SelectedItem;
                    newBusLine.FirstBusStopKey = (cbFirstBusStop.SelectedItem as BO.BusStop).BusStopKey;
                    newBusLine.LastBusStopKey = (cbLastBusStop.SelectedItem as BO.BusStop).BusStopKey;
                    runningNumber = bl.AddBusLine(newBusLine, kmToNext, timeToNext, startTime, endTime, (int)sFrequency.Value); // Inserts the new bus to the beginning of the list                 

                    // Line Stations Addition:

                    newStationA.BusLineID = runningNumber;
                    newStationA.LineStationIndex = 0;
                    newStationB.PrevStation = 0;
                    newStationA.BusStopKey = newBusLine.FirstBusStopKey;
                    newStationA.NextStation = newBusLine.LastBusStopKey;
                    bl.AddBusLineStation(newStationA);

                    newStationB.BusLineID = runningNumber;
                    newStationB.LineStationIndex = 1;
                    newStationB.BusStopKey = newBusLine.LastBusStopKey;
                    newStationB.PrevStation = newBusLine.FirstBusStopKey;
                    newStationB.NextStation = 0;
                    bl.AddBusLineStation(newStationB);
                    this.Close();
                }
            }
            catch (BO.ExceptionBL_KeyAlreadyExist)
            {
                MessageBox.Show("The bus line number you entered already exists in the company!", "Cannot add the bus stop", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Using regex to unable wrongs inputs in the text box:

        private void NumberValidationTextBoxColon(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/:]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationTextBoxDots(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/.]$");
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
            lbKmToNext.Visibility = Visibility.Collapsed;
            lbTimeToNext.Visibility = Visibility.Collapsed;
            tbKmToNext.Visibility = Visibility.Collapsed;
            tbTimeToNext.Visibility = Visibility.Collapsed;
        }

        private void cbLastBusStopSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFirstBusStop.SelectedItem != null && cbLastBusStop.SelectedItem != null)
            {
                if (!bl.IsConsecutiveExist((cbFirstBusStop.SelectedItem as BO.BusStop).BusStopKey, (cbLastBusStop.SelectedItem as BO.BusStop).BusStopKey))
                // If they are exist, or inactive, it means we know the time and distance between the two bus
                // It means the consecutive doesn't exist, and we need to manager neeed to enter the distance and time
                {
                    lbKmToNext.Visibility = Visibility.Visible;
                    lbTimeToNext.Visibility = Visibility.Visible;
                    tbKmToNext.Visibility = Visibility.Visible;
                    tbTimeToNext.Visibility = Visibility.Visible;
                }
                else
                {
                    lbKmToNext.Visibility = Visibility.Collapsed;
                    lbTimeToNext.Visibility = Visibility.Collapsed;
                    tbKmToNext.Visibility = Visibility.Collapsed;
                    tbTimeToNext.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
