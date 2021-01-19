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
    /// Interaction logic for BusLineDetailsWindow.xaml
    /// </summary>
    public partial class BusLineDetailsWindow : Window
    {
        IBL bl = BLFactory.GetBL("1"); // Calls and stores the instance of the bl interface
        BO.BusLine busLine;  // Creates a BO.busLine to store the sent item from the list in admin window

        /// <summary>
        /// Default window ctor
        /// </summary>
        public BusLineDetailsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second window ctor recieving the bus details double-clicked item
        /// </summary>
        /// <param name="item"></param>
        public BusLineDetailsWindow(object item)
        {
            InitializeComponent();
            BusLineDet.DataContext = item;
            busLine = item as BusLine;
        }

        #region Bus line add/delete

        /// <summary>
        /// The update button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Update(object sender, RoutedEventArgs e)
        {
            try
            {
                busLine.BusLineNumber = int.Parse(tbLineNumber.GetLineText(0)); // There's only updating the line number (but not area, because of logical reasons)
                bl.UpdateBusLine(busLine); // Calls the bl to update the bus line
                this.Close();
            }
            catch (BO.ExceptionBL_KeyNotFound) // In case the bus doesn't exist
            {
                MessageBox.Show("The bus line does not exist", "Unable to update");
            }
            catch (BO.ExceptionBL_Inactive) // In case the bus isn't active
            {
                MessageBox.Show("The bus line is inactive", "Unable to delete");
            }
            catch // For other exceptions
            {
                MessageBox.Show("An unexpected problem occured", "Unable to delete");
            }
        }

        /// <summary>
        /// The delete button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            // Asks if the admin surely wants to delete the object:
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this bus line?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
                try
                {
                    bl.DeleteBusLine(busLine.BusLineID); // Calls the bl to the delete the bus line
                    this.Close();
                }
                catch (BO.ExceptionBL_KeyNotFound) // In case the bus line doesn't exist
                {
                    MessageBox.Show("The bus line does not exist", "Unable to delete");
                }
                catch (BO.ExceptionBL_Inactive) // In case the bus line is inactive (already been delted)
                {
                    MessageBox.Show("The bus line has been already deleted", "Unable to delete");
                }
                catch // For other exceptions
                {
                    MessageBox.Show("An unexpected problem occured", "Unable to delete");
                }

        }

        #endregion

        #region Station deletion

        // Bool fields for grid and style manipulations:
        bool isAddStationInProcess = false;
        bool isDeleteStationInProcess = false;
        bool mustUpdateGapA = false;
        bool mustUpdateGapB = false;

        /// <summary>
        /// Delete line station click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_DeleteStation(object sender, RoutedEventArgs e)
        {
            BO.BusLineStation chosenStation = (lvLineStations.SelectedValue as BusLineStation); // Stores the selected station to deleted
            if (isDeleteStationInProcess == false) // If the deletion only being started
            {
                try
                {
                    isDeleteStationInProcess = true; // Needed for the grid and radio buttons manipulations
                    bAddStation.IsEnabled = false; // The button is disabled while waiting for the next selections

                    // In case a station was actually selected is in the middle:
                    if (chosenStation != null && chosenStation.PrevStation != 0 && chosenStation.NextStation != 0)
                    {
                        // Checks by bl if the consecutive stations object of the stations on the sides are existing:
                        if (!bl.IsConsecutiveExist(chosenStation.PrevStation, chosenStation.NextStation))
                        {
                            // Makes the update grid visible and manipulates the text boxes accordingly:
                            gUpdateConsecutive.Visibility = Visibility.Visible;
                            tbUpdateKmB.Visibility = Visibility.Hidden;
                            tbUpdateTimeB.Visibility = Visibility.Hidden;
                            tbUpdateKM.Text = "0";
                            tbUpdateTime.Text = "hh:mm:ss";
                            lbGapA.Content = chosenStation.PrevStation + " -> " + chosenStation.NextStation;
                            tbDeleteStation.Text = "Submit changes";
                        }
                        else // If the consecutive stations exist, only need to delete the bus line station trough bl
                        {
                            bl.DeleteBusLineStation(busLine.BusLineID, chosenStation.BusStopKey, TimeSpan.FromMinutes(0), 0); // Sends the bus line id, the bus stop key and time span and km equals to 0
                            stationDeletionEndProcess(); // The proccess ended, and the function will update the relevant grids
                        }
                    }
                    else if (chosenStation.PrevStation == 0 || chosenStation.NextStation == 0) // It means the station is in the end or the beggining, and no need to update the consecutive stations
                    {
                        bl.DeleteBusLineStation(busLine.BusLineID, chosenStation.BusStopKey, TimeSpan.FromMinutes(0), 0); // Sends the bus line id, the bus stop key and time span and km equals to 0
                        stationDeletionEndProcess(); // The proccess ended, and the function will update the relevant grids
                    }
                }
                catch (BO.ExceptionBL_LessThanThreeStation) // Unable to delete if there are only two stations left!
                {
                    MessageBox.Show("A bus line must contain at least two stations", "Cannot delete station", MessageBoxButton.OK, MessageBoxImage.Warning);
                    stationDeletionEndProcess(); // The proccess ended, and the function will update the relevant grids
                }
                catch
                {
                    MessageBox.Show("An unexpected problem occured", "Unable to delete");
                }

            }
            else if (tbDeleteStation.Text == "Submit changes") // If it need to submit changes, it means there is need to update consecutive
            {
                if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate)  // Checks the string by tryparse
                    || timeUpdate == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdate) || kmUpdate == 0) // If there is need to update, the km and time shoudln't be zero, and bl checks the logic of the timeSpan
                {
                    MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else // If the input correct, it deletes trough the bl
                {
                    bl.DeleteBusLineStation(busLine.BusLineID, chosenStation.BusStopKey, timeUpdate, kmUpdate);
                    stationDeletionEndProcess(); // The proccess ended, and the function will update the relevant grids
                }
            }
        }

        /// <summary>
        /// A function made for list context, grid and buttons style manipulations, when the deletion proccess ends
        /// </summary>
        private void stationDeletionEndProcess()
        {
            BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID);
            busLine = bl.GetBusLine(busLine.BusLineID); // Gets again the bus line for the next actions if needed
            gUpdateConsecutive.Visibility = Visibility.Collapsed;
            bDeleteStation.IsEnabled = false;
            isDeleteStationInProcess = false;
            bAddStation.IsEnabled = true;
            tbDeleteStation.Text = "Delete the station";
        }

        /// <summary>
        /// lvStations selection changed - a selection will end an existing proccess
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvStationsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isDeleteStationInProcess || isAddStationInProcess) // If in proccess, it will end, if not, it will able the deletion button
            {
                stationDeletionEndProcess();
                stationAdditionEndProcess();
            }
            else
                bDeleteStation.IsEnabled = true;
        }

        #endregion

        #region Station addition

        /// <summary>
        /// new station selection changed event - makes grid manipulations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newStationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Grids visibility update:

            gUpdateConsecutive.Visibility = Visibility.Collapsed;
            gChoosePrevStation.Visibility = Visibility.Collapsed;

            // Radio buttons update to delete previous changes:

            rbFirst.IsEnabled = true;
            rbMiddle.IsEnabled = true;
            rbLast.IsEnabled = true;

            rbFirst.IsChecked = false;
            rbMiddle.IsChecked = false;
            rbLast.IsChecked = false;
        }

        /// <summary>
        /// First radio button check event - the admin wants to add the bus stop to the head of the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbFirstCheck(object sender, RoutedEventArgs e)
        {
            BO.BusStop chosenBusStop = cbChooseNewStation.SelectedItem as BO.BusStop; // Storing the selected bus stop

            // Grids visibility update:
            gChoosePrevStation.Visibility = Visibility.Collapsed;
            gUpdateConsecutive.Visibility = Visibility.Collapsed;

            try
            {
                if (chosenBusStop != null)
                {
                    // Asks the bl if the consecutive first and new exist:
                    bool isConsecutiveExist = bl.IsConsecutiveExist(chosenBusStop.BusStopKey, busLine.FirstBusStopKey);
                    // If bus stop has been chosen and it has no consecutive to the head bus station
                    // If the are already consecutive stations, the admin will simply press submit changes
                    if (!isConsecutiveExist)
                    {
                        // Boolean gaps and button update:
                        mustUpdateGapA = true;
                        mustUpdateGapB = false;
                        bAddStation.IsEnabled = true;
                        tbAddStation.Text = "Submit changes";

                        // Visibility updates:
                        gUpdateConsecutive.Visibility = Visibility.Visible;
                        lbGapB.Visibility = Visibility.Hidden;
                        tbUpdateKmB.Visibility = Visibility.Hidden;
                        tbUpdateTimeB.Visibility = Visibility.Hidden;

                        // Text updates:
                        tbUpdateKM.Text = "0";
                        tbUpdateTime.Text = "hh:mm:ss";
                        lbGapA.Content = chosenBusStop.BusStopKey + " -> " + busLine.FirstBusStopKey;
                    }
                    else // No need to update consecutive, only enables the button
                    {
                        bAddStation.IsEnabled = true;
                        tbAddStation.Text = "Submit changes";
                    }
                }
            }
            catch // For unexpected issues
            {
                MessageBox.Show("An unexpected problem occured", "Unable to delete");
            }
        }

        /// <summary>
        /// Middle radio button check event - the admin wants to add the bus stop to the middle of the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbMiddleCheck(object sender, RoutedEventArgs e)
        {
            BO.BusStop chosenBusStop = cbChooseNewStation.SelectedItem as BO.BusStop; // Stores the selected bus stop
            gUpdateConsecutive.Visibility = Visibility.Collapsed; // Collapsing in case it was open
            bAddStation.IsEnabled = false; // Because the admin need to choose the station before the new one
            try
            {
                if (chosenBusStop != null) // In case the first station was actually chosen
                {
                    // Enables to choose the station before:
                    cbChoosePrevStation.Text = "After which station you wish to place the new station?";
                    gChoosePrevStation.Visibility = Visibility.Visible;
                    cbChoosePrevStation.ItemsSource = from station
                                                      in busLine.LineStations
                                                          // The query excludes the last station (for placing to the last, there is a radio button for last)
                                                      where station.BusStopKey != busLine.LastBusStopKey
                                                      select station;
                }
            }
            catch // For unexpected issues
            {
                MessageBox.Show("An unexpected problem occured", "Unable to delete");
            }
        }

        /// <summary>
        /// Last radio button check event - the admin wants to add the bus stop to the end of the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbLastCheck(object sender, RoutedEventArgs e)
        {
            BO.BusStop chosenBusStop = cbChooseNewStation.SelectedItem as BO.BusStop;  // Stores the selected bus stop
            gChoosePrevStation.Visibility = Visibility.Collapsed; // Closes the prev station grid if was visible
            gUpdateConsecutive.Visibility = Visibility.Collapsed; // Closes the consecutive if was visible

            try
            {
                if (chosenBusStop != null)
                {
                    bool isConsecutiveExist = bl.IsConsecutiveExist(busLine.LastBusStopKey, chosenBusStop.BusStopKey);
                    if (!isConsecutiveExist)
                    {
                        // Boolean gaps and button update:
                        mustUpdateGapA = true;
                        mustUpdateGapB = false;
                        tbAddStation.Text = "Submit changes";
                        bAddStation.IsEnabled = true;

                        // Visibility updates:
                        gUpdateConsecutive.Visibility = Visibility.Visible;
                        lbGapB.Visibility = Visibility.Hidden;
                        tbUpdateKmB.Visibility = Visibility.Hidden;
                        tbUpdateTimeB.Visibility = Visibility.Hidden;

                        // Text updates:
                        tbUpdateKM.Text = "0";
                        tbUpdateTime.Text = "hh:mm:ss";
                        lbGapA.Content = busLine.LastBusStopKey + " -> " + chosenBusStop.BusStopKey;

                    }
                    else // No need to update consecutive, only enables the button
                    {
                        tbAddStation.Text = "Submit changes";
                        bAddStation.IsEnabled = true;
                    }
                }
            }
            catch // For unexpected issues
            {
                MessageBox.Show("An unexpected problem occured", "Unable to delete");
            }
        }

        /// <summary>
        /// previous station selection changed event - for grid manipulations in accordance with with consecutive stations needs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prevStationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.BusStop chosenBusStop = cbChooseNewStation.SelectedItem as BO.BusStop; // Stores the selected bus stop
            BO.BusLineStation chosenPrevStation = cbChoosePrevStation.SelectedItem as BO.BusLineStation; // Stores the selected previous station

            try
            {
                if (chosenBusStop != null && chosenPrevStation != null) // Checks if the bus stops were actually selected
                {
                    bool isConsecutiveExistA = bl.IsConsecutiveExist(chosenPrevStation.BusStopKey, chosenBusStop.BusStopKey);
                    bool isConsecutiveExistB = bl.IsConsecutiveExist(chosenBusStop.BusStopKey, chosenPrevStation.NextStation);
                    if (!isConsecutiveExistA && !isConsecutiveExistB) // in case the both consecutive doesn't exist, there is a need to create them
                    {
                        // Boolean gaps and button update:
                        mustUpdateGapA = true;
                        mustUpdateGapB = true;
                        lbGapB.Content = chosenBusStop.BusStopKey + " -> " + chosenPrevStation.NextStation;
                        tbAddStation.Text = "Submit changes";
                        bAddStation.IsEnabled = true;

                        // Visibility updates:
                        gUpdateConsecutive.Visibility = Visibility.Visible;
                        lbGapB.Visibility = Visibility.Visible;
                        tbUpdateKmB.Visibility = Visibility.Visible;
                        tbUpdateTimeB.Visibility = Visibility.Visible;

                        // Text updates:
                        tbUpdateKM.Text = "0";
                        tbUpdateTime.Text = "hh:mm:ss";
                        tbUpdateKmB.Text = "0";
                        tbUpdateTimeB.Text = "hh:mm:ss";
                        lbGapA.Content = chosenPrevStation.BusStopKey + " -> " + chosenBusStop.BusStopKey;

                    }
                    else if (!isConsecutiveExistA)  // in case only the first pair of consecutive doesn't exist, there is a need to create them
                    {
                        // Boolean gaps and button update:
                        mustUpdateGapA = true;
                        tbAddStation.Text = "Submit changes";
                        bAddStation.IsEnabled = true;

                        // Visibility updates:
                        gUpdateConsecutive.Visibility = Visibility.Visible;
                        lbGapB.Visibility = Visibility.Hidden;
                        tbUpdateKmB.Visibility = Visibility.Hidden;
                        tbUpdateTimeB.Visibility = Visibility.Hidden;

                        // Text updates:
                        tbUpdateKM.Text = "0";
                        tbUpdateTime.Text = "hh:mm:ss";
                        lbGapA.Content = chosenPrevStation.BusStopKey + " -> " + chosenBusStop.BusStopKey;

                    }
                    else if (!isConsecutiveExistB)  // in case only the second pair of consecutive doesn't exist, there is a need to create them
                    {
                        // Boolean gaps and button update:
                        mustUpdateGapB = true;
                        tbAddStation.Text = "Submit changes";
                        bAddStation.IsEnabled = true;

                        // Visibility updates:
                        gUpdateConsecutive.Visibility = Visibility.Visible;
                        lbGapB.Visibility = Visibility.Hidden;
                        tbUpdateKmB.Visibility = Visibility.Hidden;
                        tbUpdateTimeB.Visibility = Visibility.Hidden;

                        // Text updates:
                        tbUpdateKmB.Text = "0";
                        tbUpdateTimeB.Text = "hh:mm:ss";
                        lbGapB.Content = chosenBusStop.BusStopKey + " -> " + chosenPrevStation.NextStation;

                    }
                    else // // in case the both pairs of consecutive stations exist, there is only left to click the button
                    {
                        tbAddStation.Text = "Submit changes";
                        bAddStation.IsEnabled = true;
                    }
                }
            }
            catch // For unexpected issues
            {
                MessageBox.Show("An unexpected problem occured", "Unable to delete");
            }

        }

        /// <summary>
        /// Add station button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_AddStation(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.BusStop chosenBusStop = cbChooseNewStation.SelectedItem as BO.BusStop; // Stores the new chosen bus stop
                BO.BusLineStation chosenPrevStation = cbChoosePrevStation.SelectedItem as BO.BusLineStation; // Stores the new chosen bus stop
                BO.BusLineStation newStation = new BO.BusLineStation(); // Creats a new bus line station to add later in this method

                // Beginning of addition (BEFORE ANY RADIO BUTTON HAS BEEN CLICKED)
                if (!isAddStationInProcess && !isDeleteStationInProcess)
                {
                    // The proccess starts and the buttons being unabled
                    isAddStationInProcess = true;
                    gChooseNewStation.Visibility = Visibility.Visible;
                    bDeleteStation.IsEnabled = false;
                    bAddStation.IsEnabled = false;

                    // The linq query makes sure the combo box of new bus stops won't have exisiting stations in the bus line:
                    cbChooseNewStation.ItemsSource = bl.GetAllBusStops().Where(f => !busLine.LineStations.Any(x => x.BusStopKey == f.BusStopKey));
                }

                // A quick explanation of the process:
                // During the stations and radio buttons choosing, the booleans of mustUpdateGapA/B
                // were changed, so now the code can determine how to send the obtained info about the consecutive stations

                // Submiting the changes, if the addition proccess is on and the chosen bus isn't null:
                else if (isAddStationInProcess == true && chosenBusStop != null)
                {

                    // Adding the line station to the head of the route:
                    if (rbFirst.IsChecked == true)
                    {
                        // Initializing the station fields:
                        newStation.BusLineID = busLine.BusLineID;
                        newStation.BusStopKey = chosenBusStop.BusStopKey;
                        newStation.NextStation = busLine.FirstBusStopKey;

                        // Because the new station is at the head:
                        newStation.PrevStation = 0;

                        if (mustUpdateGapA == true) // The admin was asked to fill the consecutive stations info (from the new to the first in the list)
                        {
                            // Validity of input check:
                            if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdate) || kmUpdate == 0)
                            {
                                MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                newStation.DistanceToNext = kmUpdate;
                                newStation.TimeToNext = timeUpdate;
                                bl.AddBusLineStation(newStation, TimeSpan.FromMinutes(0), 0); // Calls the bl to add the bus line station and the consecutive if needed (by km and time to next stored in the newStation properties)
                                stationAdditionEndProcess(); // The addition proccess ends
                            }
                        }
                        else // The consecutive already exist
                        {
                            newStation.DistanceToNext = 0;
                            newStation.TimeToNext = TimeSpan.FromMinutes(0);
                            bl.AddBusLineStation(newStation, TimeSpan.FromMinutes(0), 0); // Calls the bl to add the bus line station and the consecutive if needed (by km and time to next stored in the newStation properties)
                            stationAdditionEndProcess(); // The addition proccess ends
                        }
                    }


                    // Adding the line station to the end of the route:
                    else if (rbLast.IsChecked == true)
                    {
                        // Initializing the station fields:
                        newStation.BusLineID = busLine.BusLineID;
                        newStation.BusStopKey = chosenBusStop.BusStopKey;
                        newStation.PrevStation = busLine.LastBusStopKey;

                        // Because the new station is at the end:
                        newStation.NextStation = 0;
                        newStation.DistanceToNext = 0;
                        newStation.TimeToNext = TimeSpan.FromMinutes(0);

                        if (mustUpdateGapA == true) // The admin was asked to fill the consecutive stations info (from the last to the new in the list)
                        {
                            // Validity of input check:
                            if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdate) || kmUpdate == 0)
                            {
                                MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                bl.AddBusLineStation(newStation, timeUpdate, kmUpdate); // Sends the new station to the bl addition function, with the info about the km and time from the previous
                                stationAdditionEndProcess();
                            }
                        }
                        else
                        {
                            bl.AddBusLineStation(newStation, TimeSpan.FromMinutes(0), 0); // Calls the bl to add the bus line station (no consecutive addition needed)                 
                            stationAdditionEndProcess();
                        }

                    }

                    // Adding the line station to the middle of the route:
                    else if (rbMiddle.IsChecked == true && chosenPrevStation != null) // In case the previous station was actually chosen
                    {
                        // Initializing the station fields:
                        newStation.BusLineID = busLine.BusLineID;
                        newStation.BusStopKey = chosenBusStop.BusStopKey;
                        newStation.BusStopName = chosenBusStop.BusStopName;
                        newStation.LineStationIndex = chosenPrevStation.LineStationIndex + 1; // The index is determined now, before sending the newStation to bl (unlike when the new station matched the head or to the end)
                        newStation.PrevStation = chosenPrevStation.BusStopKey;
                        newStation.NextStation = chosenPrevStation.NextStation;

                        // The admin was asked to fill a pair of consecutive stations info (from the previous to the new one, and from the new one to the next in the list)
                        if (mustUpdateGapA == true && mustUpdateGapB == true)
                        {
                            // Input validation check:
                            if (!Double.TryParse(tbUpdateKmB.GetLineText(0), out double kmUpdateB) || !TimeSpan.TryParse(tbUpdateTimeB.GetLineText(0), out TimeSpan timeUpdateB) || timeUpdateB == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdateB) || kmUpdateB == 0 ||
                                !Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdate) || kmUpdate == 0)
                            {
                                MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                // The fields updateded and will be used for the consecutive stations creation later in the bl:
                                newStation.TimeToNext = timeUpdateB;
                                newStation.DistanceToNext = kmUpdateB;
                                bl.AddBusLineStation(newStation, timeUpdate, kmUpdate); // The last two paramters represent the first pair of consecutive stations information
                                stationAdditionEndProcess(); // The addition proccess ends
                            }
                        }

                        // The admin was asked to fill the consecutive stations info (from the prev to the new one)
                        else if (mustUpdateGapA == true)
                        {
                            // Input validation check:
                            if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdate) || kmUpdate == 0)
                            {
                                MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                bl.AddBusLineStation(newStation, timeUpdate, kmUpdate); // The last two paramters represent the first pair of consecutive stations information
                                stationAdditionEndProcess(); // The addition proccess ends
                            }
                        }

                        // The admin was asked to fill the consecutive stations info (from the new one to the next in the list)
                        else if (mustUpdateGapB == true)
                        {
                            // Input validation checked:
                            if (!Double.TryParse(tbUpdateKmB.GetLineText(0), out double kmUpdateB) || !TimeSpan.TryParse(tbUpdateTimeB.GetLineText(0), out TimeSpan timeUpdateB) || timeUpdateB == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdateB) || kmUpdateB == 0)
                            {
                                MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                // The fields updateded and will be used for the consecutive stations creation later in the bl:s
                                newStation.TimeToNext = timeUpdateB;
                                newStation.DistanceToNext = kmUpdateB;
                                bl.AddBusLineStation(newStation, TimeSpan.FromMinutes(0), 0);  // The last two paramters are zero because there is no need to create consecutive stations from the previous to the new
                                stationAdditionEndProcess();  // The addition proccess ends
                            }
                        }
                        else // No need to update the gaps (there are already consecutive stations for both sides!)
                        {
                            bl.AddBusLineStation(newStation, TimeSpan.FromMinutes(0), 0);   // The last two paramters are zero because there is no need to create consecutive stations from the previous to the new
                            stationAdditionEndProcess(); // The addition proccess ends
                        }
                    }

                }
            }
            catch // In case there are unexpected issues
            {
                MessageBox.Show("An unexpected problem occured", "Unable to delete");
            }
        }

        /// <summary>
        /// A function made for list context, grid and buttons style manipulations, when the addition proccess ends
        /// </summary>
        private void stationAdditionEndProcess()
        {
            // Data context updates:
            BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID);
            busLine = bl.GetBusLine(busLine.BusLineID);

            // Grid visibility updates:
            gUpdateConsecutive.Visibility = Visibility.Collapsed;
            gChooseNewStation.Visibility = Visibility.Collapsed;
            gChoosePrevStation.Visibility = Visibility.Collapsed;


            // Boolean variables updates:
            mustUpdateGapA = false;
            mustUpdateGapB = false;
            isAddStationInProcess = false;

            // Radio buttons and add button updates::
            rbFirst.IsEnabled = false;
            rbMiddle.IsEnabled = false;
            rbLast.IsEnabled = false;
            bAddStation.IsEnabled = true;

            // Text updates:
            cbChoosePrevStation.Text = "After which station you wish to place the new station?";
            cbChooseNewStation.Text = "Choose the new station";
            tbAddStation.Text = "Add a station";
        }




        #endregion

        #region Schedule delete/add methods

        /// <summary>
        /// List view - selection change event collapsing the add departure grid and enables the deletion button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvScheduleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gAddDeparture.Visibility = Visibility.Collapsed;
            bDeleteDeparture.IsEnabled = true;
        }

        /// <summary>
        /// Add departure button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_AddDeparture(object sender, RoutedEventArgs e)
        {
            if (gAddDeparture.Visibility != Visibility.Visible) // If the grid of add departure isn't visible, it makes it visible
                gAddDeparture.Visibility = Visibility.Visible;
            else
                try
                {
                    if (TimeSpan.TryParse(tbAddDeparture.GetLineText(0), out TimeSpan timeDeparture) && // Checks if the input correct
                        !bl.isTimeSpanInvalid(timeDeparture)) // Calls the logical check of the timeSpan entered
                    {
                        bl.AddLineDeparture(timeDeparture, busLine.BusLineID); // Adds by the bl
                        BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID); // Updates the list view of the departures
                        gAddDeparture.Visibility = Visibility.Collapsed; // The grid of adding collapsing 
                        tbAddDeparture.Text = "hh:mm:ss";
                    }
                    else
                        MessageBox.Show("You have entered a wrong time departure!");
                }
                catch (ExceptionBL_KeyAlreadyExist) // In case the time departure already exist
                {
                    MessageBox.Show("This time departure already exist!");
                }
                catch // For other exceptions
                {
                    MessageBox.Show("An unexpected problem occured", "Unable to delete");
                }
        }

        /// <summary>
        /// Delete departure button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_DeleteDeparture(object sender, RoutedEventArgs e)
        {
            try
            {
                gAddDeparture.Visibility = Visibility.Collapsed; // Collapsing the addition grid
                if (lvSchedule.SelectedValue != null) // Checks that a selection actually has been made
                    bl.DeleteLineDeparture((TimeSpan)lvSchedule.SelectedValue, busLine.BusLineID); // Calls bl for the deletion of the list view selected item
                BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID); // Updates the list view
                bDeleteDeparture.IsEnabled = false;
            }
            catch (BO.ExceptionBL_KeyNotFound ex) // In case the time departure doesn't exist
            {
                MessageBox.Show("This time departure doesn't exist!", ex.Message);
            }
            catch (BO.ExceptionBL_Inactive ex) // In case the time departure is inactive
            {
                MessageBox.Show("This time departure already is inactive!", ex.Message);
            }
            catch // For other exceptions
            {
                MessageBox.Show("An unexpected problem occured", "Unable to delete");
            }
        }

        #endregion

        #region input validations

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

        /// <summary>
        /// Preview keyboard input to numbers with colons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBoxColon(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/:]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Preview keyboard input to numbers with dots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBoxDots(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/.]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion
    }
}
