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
        BO.BusLine newBusLine;
        BO.BusLineStation newStationA;
        BO.BusLineStation newStationB;
        BO.ConsecutiveStations newConStations;
        BO.LineDeparture newLineDeparture;

        public AddBusLineWindow()
        {
            InitializeComponent();
            newBusLine = new BO.BusLine();
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
                if (!Double.TryParse(tbKmToNext.GetLineText(0), out double kmToNext) || !TimeSpan.TryParse(tbTimeToNext.GetLineText(0), out TimeSpan timeToNext)
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
                    bl.AddBusLine(newBusLine);   // Inserts the new bus to the beginning of the list                 

                    // Line Stations Addition:

                    //newStationA.BusLineID = ;
                    newStationA.LineStationIndex = 0;
                    newStationA.BusStopKey = newBusLine.FirstBusStopKey;
                    newStationA.NextStation = newBusLine.LastBusStopKey;

                    //newStationB.BusLineID = ;
                    newStationB.LineStationIndex = 1;
                    newStationB.BusStopKey = newBusLine.LastBusStopKey;
                    newStationB.NextStation = 0;

                    // Consecutive line stations:
                    newConStations.BusStopKeyA = newBusLine.FirstBusStopKey;
                    newConStations.BusStopKeyB = newBusLine.LastBusStopKey;
                    newConStations.Distance = kmToNext;
                    newConStations.TravelTime = timeToNext;

                    // Departure:
                    //newLineDeparture.BusLineID = ;
                    newLineDeparture.StartTime = startTime;
                    newLineDeparture.EndTime = endTime;
                    newLineDeparture.Frequency = (int)sFrequency.Value;
                    this.Close();
                }
            }
            catch (BO.ExceptionBLBadLicense)
            {
                MessageBox.Show("The bus stop code you entered already exists in the company!", "Cannot add the bus stop", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        }

        private void cbLastBusStopSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
