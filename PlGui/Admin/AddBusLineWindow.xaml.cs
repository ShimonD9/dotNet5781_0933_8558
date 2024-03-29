﻿using System;
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
        IBL bl = BLFactory.GetBL("1"); // Calls and stores the instance of the bl interface

        // Appropriate BO objects creation:
        BusLine newBusLine = new BO.BusLine();
        BusLineStation newStationA = new BusLineStation();
        BusLineStation newStationB = new BusLineStation();
        bool mustUpdateGap = false; // To know if consecutive stations must be created

        /// <summary>
        /// Default window ctor
        /// </summary>
        public AddBusLineWindow()
        {
            InitializeComponent();
            cbArea.ItemsSource = Enum.GetValues(typeof(BO.Enums.AREA)); // Initializing the area combo box with the area enum values
            cbLastBusStop.IsEnabled = false;                            // The last bus combo box is unabled 
            cbFirstBusStop.ItemsSource = bl.GetAllBusStops().OrderBy(busStop => busStop.BusStopKey); // The first bus combo box ordered by the bus stop key
        }


        /// <summary>
        /// The adding bus button method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // First initialization of km and timeSpan between two stations (for consecutive stations purposes)
                double kmToNext = 0;
                TimeSpan timeToNext = new TimeSpan(0, 0, 0);

                // Validity check of inputs and combo box selections:
                if (cbFirstBusStop.SelectedItem == null || cbLastBusStop.SelectedItem == null || cbArea.SelectedItem == null || String.IsNullOrEmpty(tbLineNumber.GetLineText(0)) ||
                    mustUpdateGap && (!Double.TryParse(tbKmToNext.GetLineText(0), out kmToNext) || kmToNext == 0) || // In case must update the gap, but the text is invalid or the distance is zero
                    mustUpdateGap && (!TimeSpan.TryParse(tbTimeToNext.GetLineText(0), out timeToNext) || bl.isTimeSpanInvalid(timeToNext)))  // Same as above
                {
                    MessageBox.Show("You didn't fill correctly all the required information", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Bus line properties filled:
                    newBusLine.BusLineNumber = int.Parse(tbLineNumber.GetLineText(0));
                    newBusLine.Area = (Enums.AREA)cbArea.SelectedItem;
                    newBusLine.FirstBusStopKey = (cbFirstBusStop.SelectedItem as BO.BusStop).BusStopKey;
                    newBusLine.LastBusStopKey = (cbLastBusStop.SelectedItem as BO.BusStop).BusStopKey;

                    // First bus stop properties filled:
                    newStationA.LineStationIndex = 0;
                    newStationA.PrevStation = 0;
                    newStationA.BusStopKey = newBusLine.FirstBusStopKey;
                    newStationA.NextStation = newBusLine.LastBusStopKey;
                    newStationA.DistanceToNext = kmToNext; // Used for the consecutive station creation in Blimp
                    newStationA.TimeToNext = timeToNext;  // Used for the consecutive station creation in Blimp

                    // Second bus stop properties filled
                    newStationB.LineStationIndex = 1;
                    newStationB.BusStopKey = newBusLine.LastBusStopKey;
                    newStationB.PrevStation = newBusLine.FirstBusStopKey;
                    newStationB.NextStation = 0;

                    // The bus line is being added
                    bl.AddBusLine(newBusLine, newStationA, newStationB); // Inserts the new bus to the beginning of the list (The consecutive updated there)            
                    this.Close();
                }
            }
            catch (BO.ExceptionBL_KeyAlreadyExist) // In case the bus line already exist
            {
                MessageBox.Show("The bus line number you entered already exists in the company!", "Cannot add the bus stop", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)// For unexpected issues
            {
                MessageBox.Show("An unexpected problem occured: " + ex.Message, "Cannot add the bus line");
            }
        }


        /// <summary>
        /// The first bus stop item selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbFirstBusStopSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mustUpdateGap = false;
            cbLastBusStop.IsEnabled = true;
            try
            {
                if (cbFirstBusStop.SelectedItem != null) // In case it will be changed to null because of the second combo box method
                    cbLastBusStop.ItemsSource = bl.GetAllBusStops()
                        .Where(busStop => busStop.BusStopKey != (cbFirstBusStop.SelectedItem as BO.BusStop).BusStopKey)
                        .OrderBy(busStop => busStop.BusStopKey);
            }
            catch (Exception ex)// For unexpected issues
            {
                MessageBox.Show("An unexpected problem occured: " + ex.Message, "Error!");
            }
            // Visiblity changes:
            lbKmToNext.Visibility = Visibility.Collapsed;
            lbTimeToNext.Visibility = Visibility.Collapsed;
            tbKmToNext.Visibility = Visibility.Collapsed;
            tbTimeToNext.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// The last bus stop item selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbLastBusStopSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFirstBusStop.SelectedItem != null && cbLastBusStop.SelectedItem != null)
            {
                try
                {
                    if (!bl.IsConsecutiveExist((cbFirstBusStop.SelectedItem as BO.BusStop).BusStopKey, (cbLastBusStop.SelectedItem as BO.BusStop).BusStopKey))
                    // If they are exist, or inactive, it means we know the time and distance between the two bus
                    // It means the consecutive doesn't exist, and we need to manager neeed to enter the distance and time
                    {
                        mustUpdateGap = true;
                        lbKmToNext.Visibility = Visibility.Visible;
                        lbTimeToNext.Visibility = Visibility.Visible;
                        tbKmToNext.Visibility = Visibility.Visible;
                        tbTimeToNext.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        mustUpdateGap = false;
                        lbKmToNext.Visibility = Visibility.Collapsed;
                        lbTimeToNext.Visibility = Visibility.Collapsed;
                        tbKmToNext.Visibility = Visibility.Collapsed;
                        tbTimeToNext.Visibility = Visibility.Collapsed;
                    }
                }
                catch (Exception ex)// For unexpected issues
                {
                    MessageBox.Show("An unexpected problem occured: " + ex.Message, "Error!");
                }
            }
        }

        #region Using regex to unable wrongs inputs in the text box:

        /// <summary>
        /// Preview keyboard input to numbers with colons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberValidationTextBoxColon(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/:]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Preview keyboard input to numbers with dots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberValidationTextBoxDots(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/.]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Preview keyboard input to numbers only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberValidationTextBoxNoDots(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

    }
}
