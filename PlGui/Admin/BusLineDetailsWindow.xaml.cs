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
            catch (BO.ExceptionBL_UnexpectedProblem) // For other exceptions
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
                catch (BO.ExceptionBL_UnexpectedProblem) // For other exceptions
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
                    MessageBox.Show("There are only two station in the line", "Cannot delete station", MessageBoxButton.OK, MessageBoxImage.Warning);
                    stationDeletionEndProcess(); // The proccess ended, and the function will update the relevant grids
                }
                catch (BO.ExceptionBL_UnexpectedProblem)
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
        private void Button_AddStation(object sender, RoutedEventArgs e)
        {
            BO.BusStop chosenBusStop = cbChooseNewStation.SelectedItem as BO.BusStop;
            BO.BusLineStation chosenPrevStation = cbChoosePrevStation.SelectedItem as BO.BusLineStation;
            BO.BusLineStation newStation = new BO.BusLineStation();
 

            // Beginning of addition:
            if (!isAddStationInProcess && !isDeleteStationInProcess)
            {
                isAddStationInProcess = true;
                gChooseNewStation.Visibility = Visibility.Visible;
                bDeleteStation.IsEnabled = false;
                bAddStation.IsEnabled = false;
                cbChooseNewStation.ItemsSource = bl.GetAllBusStops().Where(f => !busLine.LineStations.Any(x => x.BusStopKey == f.BusStopKey)); 
            }

            // Submiting the changes:
            else if (isAddStationInProcess == true && chosenBusStop != null)
            {
                if (rbFirst.IsChecked == true) // Adding the line station to the head of the route
                {
                    newStation.BusLineID = busLine.BusLineID;
                    newStation.BusStopKey = chosenBusStop.BusStopKey;
                    newStation.NextStation = busLine.FirstBusStopKey;
                    newStation.PrevStation = 0;
                    if (mustUpdateGapA == true)
                    {
                        if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdate) || kmUpdate == 0)
                        {
                            MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            newStation.DistanceToNext = kmUpdate;
                            newStation.TimeToNext = timeUpdate;
                        }
                    }
                    else
                    {
                        newStation.DistanceToNext = 0;
                        newStation.TimeToNext = TimeSpan.FromMinutes(0);
                    }
                    bl.AddBusLineStation(newStation, TimeSpan.FromMinutes(0), 0);
                    stationAdditionEndProcess();
                }
                else if (rbLast.IsChecked == true)  // Adding the line station to the end of the route
                {
                    newStation.BusLineID = busLine.BusLineID;
                    newStation.BusStopKey = chosenBusStop.BusStopKey;
                    newStation.NextStation = 0;
                    newStation.PrevStation = busLine.LastBusStopKey;
                    newStation.DistanceToNext = 0;
                    newStation.TimeToNext = TimeSpan.FromMinutes(0);
                    if (mustUpdateGapA == true)
                    {
                        if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdate) || kmUpdate == 0)
                        {
                            MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            bl.AddBusLineStation(newStation, timeUpdate, kmUpdate);
                            stationAdditionEndProcess();
                        }
                    }
                    else
                    {
                        bl.AddBusLineStation(newStation, TimeSpan.FromMinutes(0), 0);
                    }
                }
                else if (rbMiddle.IsChecked == true && chosenPrevStation != null)
                {

                    newStation.BusLineID = busLine.BusLineID;
                    newStation.BusStopKey = chosenBusStop.BusStopKey;
                    newStation.BusStopName = chosenBusStop.BusStopName;
                    newStation.LineStationIndex = chosenPrevStation.LineStationIndex + 1;
                    newStation.PrevStation = chosenPrevStation.BusStopKey;
                    newStation.NextStation = chosenPrevStation.NextStation;
                    if (mustUpdateGapA == true && mustUpdateGapB == true)
                    {
                        if (!Double.TryParse(tbUpdateKmB.GetLineText(0), out double kmUpdateB) || !TimeSpan.TryParse(tbUpdateTimeB.GetLineText(0), out TimeSpan timeUpdateB) || timeUpdateB == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdateB) || kmUpdateB == 0 ||
                            !Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdate) || kmUpdate == 0)
                        {
                            MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            newStation.TimeToNext = timeUpdateB;
                            newStation.DistanceToNext = kmUpdateB;
                            bl.AddBusLineStation(newStation, timeUpdate, kmUpdate);
                            stationAdditionEndProcess();
                        }
                    }
                    else if (mustUpdateGapA == true)
                    {
                        if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdate) || kmUpdate == 0)
                        {
                            MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            bl.AddBusLineStation(newStation, timeUpdate, kmUpdate);
                            stationAdditionEndProcess();
                        }
                    }
                    else if (mustUpdateGapB == true)
                    {
                        if (!Double.TryParse(tbUpdateKmB.GetLineText(0), out double kmUpdateB) || !TimeSpan.TryParse(tbUpdateTimeB.GetLineText(0), out TimeSpan timeUpdateB) || timeUpdateB == TimeSpan.FromMinutes(0) || bl.isTimeSpanInvalid(timeUpdateB) || kmUpdateB == 0)
                        {
                            MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            newStation.TimeToNext = timeUpdateB;
                            newStation.DistanceToNext = kmUpdateB;
                            bl.AddBusLineStation(newStation, TimeSpan.FromMinutes(0), 0);
                            stationAdditionEndProcess();
                        }
                    }
                    else // No need to update the gaps (there are consecutive stations for both sides!)
                    {
                        bl.AddBusLineStation(newStation, TimeSpan.FromMinutes(0), 0);
                        stationAdditionEndProcess();
                    }
                }

            }
        }

        /// <summary>
        /// A function made for list context, grid and buttons style manipulations, when the addition proccess ends
        /// </summary>
        private void stationAdditionEndProcess()
        {
            BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID);
            busLine = bl.GetBusLine(busLine.BusLineID);
            gUpdateConsecutive.Visibility = Visibility.Collapsed;
            gChooseNewStation.Visibility = Visibility.Collapsed;
            mustUpdateGapA = false;
            mustUpdateGapB = false;
            rbFirst.IsEnabled = false;
            rbMiddle.IsEnabled = false;
            rbLast.IsEnabled = false;
            cbChoosePrevStation.Text = "After which station you wish to place the new station?";
            cbChooseNewStation.Text = "Choose the new station";
            gChoosePrevStation.Visibility = Visibility.Collapsed;
            isAddStationInProcess = false;
            bAddStation.IsEnabled = true;
            tbAddStation.Text = "Add a station";
        }

        private void newStationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gUpdateConsecutive.Visibility = Visibility.Collapsed;
            gChoosePrevStation.Visibility = Visibility.Collapsed;
            rbFirst.IsEnabled = true;
            rbMiddle.IsEnabled = true;
            rbLast.IsEnabled = true;
            rbFirst.IsChecked = false;
            rbMiddle.IsChecked = false;
            rbLast.IsChecked = false;
        }

        private void prevStationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.BusStop chosenBusStop = cbChooseNewStation.SelectedItem as BO.BusStop;
            BO.BusLineStation chosenPrevStation = cbChoosePrevStation.SelectedItem as BO.BusLineStation;
            try
            {
                if (chosenBusStop != null && chosenPrevStation != null)
                {
                    if (!bl.IsConsecutiveExist(chosenPrevStation.BusStopKey, chosenBusStop.BusStopKey) && !bl.IsConsecutiveExist(chosenBusStop.BusStopKey, chosenPrevStation.NextStation))
                    {
                        mustUpdateGapA = true;
                        mustUpdateGapB = true;
                        gUpdateConsecutive.Visibility = Visibility.Visible;
                        lbGapB.Visibility = Visibility.Visible;
                        tbUpdateKmB.Visibility = Visibility.Visible;
                        tbUpdateTimeB.Visibility = Visibility.Visible;
                        tbUpdateKM.Text = "0";
                        tbUpdateTime.Text = "hh:mm:ss";
                        tbUpdateKmB.Text = "0";
                        tbUpdateTimeB.Text = "hh:mm:ss";
                        lbGapA.Content = chosenPrevStation.BusStopKey + " -> " + chosenBusStop.BusStopKey;
                        lbGapB.Content = chosenBusStop.BusStopKey + " -> " + chosenPrevStation.NextStation;
                        tbAddStation.Text = "Submit changes";
                        bAddStation.IsEnabled = true;
                    }
                    else if (!bl.IsConsecutiveExist(chosenPrevStation.BusStopKey, chosenBusStop.BusStopKey))
                    {
                        mustUpdateGapA = true;
                        gUpdateConsecutive.Visibility = Visibility.Visible;
                        lbGapB.Visibility = Visibility.Hidden;
                        tbUpdateKmB.Visibility = Visibility.Hidden;
                        tbUpdateTimeB.Visibility = Visibility.Hidden;
                        tbUpdateKM.Text = "0";
                        tbUpdateTime.Text = "hh:mm:ss";
                        lbGapA.Content = chosenPrevStation.BusStopKey + " -> " + chosenBusStop.BusStopKey;
                        tbAddStation.Text = "Submit changes";
                        bAddStation.IsEnabled = true;
                    }
                    else if (!bl.IsConsecutiveExist(chosenBusStop.BusStopKey, chosenPrevStation.NextStation))
                    {
                        mustUpdateGapB = true;
                        gUpdateConsecutive.Visibility = Visibility.Visible;
                        lbGapB.Visibility = Visibility.Hidden;
                        tbUpdateKmB.Visibility = Visibility.Hidden;
                        tbUpdateTimeB.Visibility = Visibility.Hidden;
                        tbUpdateKmB.Text = "0";
                        tbUpdateTimeB.Text = "hh:mm:ss";
                        lbGapB.Content = chosenBusStop.BusStopKey + " -> " + chosenPrevStation.NextStation;
                        tbAddStation.Text = "Submit changes";
                        bAddStation.IsEnabled = true;
                    }
                    else // There are consecutive stations for both sides
                    {
                        tbAddStation.Text = "Submit changes";
                        bAddStation.IsEnabled = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void rbFirstCheck(object sender, RoutedEventArgs e)
        {
            BO.BusStop chosenBusStop = cbChooseNewStation.SelectedItem as BO.BusStop;
            gChoosePrevStation.Visibility = Visibility.Collapsed;
            gUpdateConsecutive.Visibility = Visibility.Collapsed;
            try
            {
                if (chosenBusStop != null && !bl.IsConsecutiveExist(chosenBusStop.BusStopKey, busLine.FirstBusStopKey))
                {
                    mustUpdateGapA = true;
                    mustUpdateGapB = false;
                    gUpdateConsecutive.Visibility = Visibility.Visible;
                    lbGapB.Visibility = Visibility.Hidden;
                    tbUpdateKmB.Visibility = Visibility.Hidden;
                    tbUpdateTimeB.Visibility = Visibility.Hidden;
                    tbUpdateKM.Text = "0";
                    tbUpdateTime.Text = "hh:mm:ss";
                    lbGapA.Content = chosenBusStop.BusStopKey + " -> " + busLine.FirstBusStopKey;
                    tbAddStation.Text = "Submit changes";
                    bAddStation.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void rbMiddleCheck(object sender, RoutedEventArgs e)
        {
            BO.BusStop chosenBusStop = cbChooseNewStation.SelectedItem as BO.BusStop;
            gUpdateConsecutive.Visibility = Visibility.Collapsed;
            bAddStation.IsEnabled = false;
            try
            {
                if (chosenBusStop != null)
                {
                    cbChoosePrevStation.Text = "After which station you wish to place the new station?";
                    gChoosePrevStation.Visibility = Visibility.Visible;
                    cbChoosePrevStation.ItemsSource = from station in busLine.LineStations where station.BusStopKey != busLine.LastBusStopKey select station;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void rbLastCheck(object sender, RoutedEventArgs e)
        {

            gChoosePrevStation.Visibility = Visibility.Collapsed;
            gUpdateConsecutive.Visibility = Visibility.Collapsed;
            BO.BusStop chosenBusStop = cbChooseNewStation.SelectedItem as BO.BusStop;
            try
            {
                if (chosenBusStop != null && !bl.IsConsecutiveExist(busLine.LastBusStopKey, chosenBusStop.BusStopKey))
                {
                    mustUpdateGapA = true;
                    mustUpdateGapB = false;
                    gUpdateConsecutive.Visibility = Visibility.Visible;
                    lbGapB.Visibility = Visibility.Hidden;
                    tbUpdateKmB.Visibility = Visibility.Hidden;
                    tbUpdateTimeB.Visibility = Visibility.Hidden;
                    tbUpdateKM.Text = "0";
                    tbUpdateTime.Text = "hh:mm:ss";
                    lbGapA.Content = busLine.LastBusStopKey + " -> " + chosenBusStop.BusStopKey;
                    tbAddStation.Text = "Submit changes";
                    bAddStation.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
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
                catch (BO.ExceptionBL_UnexpectedProblem) // For other exceptions
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
                bl.DeleteLineDeparture((TimeSpan)lvSchedule.SelectedValue, busLine.BusLineID); // Calls bl for the deletion of the list view selected item
                BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID); // Updates the list view
            }
            catch (BO.ExceptionBL_KeyNotFound ex) // In case the time departure doesn't exist
            {
                MessageBox.Show("This time departure doesn't exist!", ex.Message);
            }
            catch (BO.ExceptionBL_Inactive ex) // In case the time departure is inactive
            {
                MessageBox.Show("This time departure already is inactive!", ex.Message);
            }
            catch (BO.ExceptionBL_UnexpectedProblem) // For other exceptions
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
