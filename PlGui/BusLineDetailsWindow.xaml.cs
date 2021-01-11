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
        BO.BusLine busLine;
        IBL bl = BLFactory.GetBL("1");
        bool isAddStationInProcess = false;
        bool isDeleteStationInProcess = false;
        bool mustUpdateGapA = false;
        bool mustUpdateGapB = false;

        public BusLineDetailsWindow()
        {
            InitializeComponent();
        }

        // A second builder, to get the item selected data
        public BusLineDetailsWindow(object item)
        {
            InitializeComponent();
            BusLineDet.DataContext = item;
            busLine = item as BusLine;
        }

        // Bus line update and delete methods:

        private void Button_Update(object sender, RoutedEventArgs e)
        {
            try
            {
                busLine.BusLineNumber = int.Parse(tbLineNumber.GetLineText(0));
                bl.UpdateBusLine(busLine);
                this.Close();
            }
            catch (BO.ExceptionBL_KeyNotFound)
            {
                MessageBox.Show("The bus line does not exist", "Unable to update");
            }
            catch (BO.ExceptionBL_Inactive)
            {
                MessageBox.Show("The bus line is inactive", "Unable to delete");
            }
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this bus line?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
                try
                {
                    bl.DeleteBusLine(busLine.BusLineID);
                    this.Close();
                }
                catch (BO.ExceptionBL_KeyNotFound)
                {
                    MessageBox.Show("The bus line does not exist", "Unable to delete");
                }
                catch (BO.ExceptionBL_Inactive)
                {
                    MessageBox.Show("The bus line already deleted", "Unable to delete");
                }

        }


        // Station delete methods:

        private void Button_DeleteStation(object sender, RoutedEventArgs e)
        {
            BO.BusLineStation chosenStation = (lvLineStations.SelectedValue as BusLineStation);
            if (isDeleteStationInProcess == false)
            {
                try
                {
                    isDeleteStationInProcess = true;
                    bAddStation.IsEnabled = false;
                    if (chosenStation != null && chosenStation.PrevStation != 0 && chosenStation.NextStation != 0)
                    {
                        if (!bl.IsConsecutiveExist(chosenStation.PrevStation, chosenStation.NextStation))
                        {
                            gUpdateConsecutive.Visibility = Visibility.Visible;
                            tbUpdateKmB.Visibility = Visibility.Hidden;
                            tbUpdateTimeB.Visibility = Visibility.Hidden;
                            tbUpdateKM.Text = "0";
                            tbUpdateTime.Text = "hh:mm:ss";
                            lbGapA.Content = chosenStation.PrevStation + " -> " + chosenStation.NextStation;
                            tbDeleteStation.Text = "Submit changes";
                        }
                        else
                        {
                            bl.DeleteBusLineStation(busLine.BusLineID, chosenStation.BusStopKey, TimeSpan.FromMinutes(0), 0);
                            stationDeletionEndProcess();
                        }
                    }
                    else if (chosenStation.PrevStation == 0 || chosenStation.NextStation == 0)
                    {
                        bl.DeleteBusLineStation(busLine.BusLineID, chosenStation.BusStopKey, TimeSpan.FromMinutes(0), 0);
                        stationDeletionEndProcess();
                    }
                }
                catch (BO.ExceptionBL_LessThanThreeStation)
                {
                    MessageBox.Show("There are only two station in the line", "Cannot delete station", MessageBoxButton.OK, MessageBoxImage.Warning);
                    stationDeletionEndProcess();
                }
                catch (Exception)
                {
                    throw;
                }

            }
            else if (tbDeleteStation.Text == "Submit changes")
            {
                if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || kmUpdate == 0)
                {
                    MessageBox.Show("You didn't fill correctly all the required information", "Cannot submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    bl.DeleteBusLineStation(busLine.BusLineID, chosenStation.BusStopKey, timeUpdate, kmUpdate);
                    BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID);
                    gUpdateConsecutive.Visibility = Visibility.Collapsed;
                    tbUpdateKmB.Visibility = Visibility.Visible;
                    tbUpdateTimeB.Visibility = Visibility.Visible;
                    tbDeleteStation.Text = "Delete";
                    bDeleteStation.IsEnabled = false;
                }
            }
        }

        private void stationDeletionEndProcess()
        {
            BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID);
            busLine = bl.GetBusLine(busLine.BusLineID);
            gUpdateConsecutive.Visibility = Visibility.Collapsed;
            bDeleteStation.IsEnabled = false;
            isDeleteStationInProcess = false;
            bAddStation.IsEnabled = true;
            tbDeleteStation.Text = "Delete the station";
        }

        private void lvStationsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isDeleteStationInProcess || isAddStationInProcess)
            {
                stationDeletionEndProcess();
                stationAdditionEndProcess();
            }
            else
                bDeleteStation.IsEnabled = true;
        }

      


        // Stations add methods:

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
                cbChooseNewStation.ItemsSource = from busStop in bl.GetAllBusStops()
                                                 where !busLine.LineStations.Any(x => x.BusStopKey == busStop.BusStopKey) // if the bus stop in use at the current line, it won't show in the combo box
                                                 select busStop;
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
                        if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || kmUpdate == 0)
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
                        if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || kmUpdate == 0)
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
                        if (!Double.TryParse(tbUpdateKmB.GetLineText(0), out double kmUpdateB) || !TimeSpan.TryParse(tbUpdateTimeB.GetLineText(0), out TimeSpan timeUpdateB) || timeUpdateB == TimeSpan.FromMinutes(0) || kmUpdateB == 0 ||
                            !Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || kmUpdate == 0)
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
                        if (!Double.TryParse(tbUpdateKM.GetLineText(0), out double kmUpdate) || !TimeSpan.TryParse(tbUpdateTime.GetLineText(0), out TimeSpan timeUpdate) || timeUpdate == TimeSpan.FromMinutes(0) || kmUpdate == 0)
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
                        if (!Double.TryParse(tbUpdateKmB.GetLineText(0), out double kmUpdateB) || !TimeSpan.TryParse(tbUpdateTimeB.GetLineText(0), out TimeSpan timeUpdateB) || timeUpdateB == TimeSpan.FromMinutes(0) || kmUpdateB == 0)
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
                }

            }
        }

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
                        tbUpdateKM.Text = "0";
                        tbUpdateTime.Text = "hh:mm:ss";
                        lbGapA.Content = chosenBusStop.BusStopKey + " -> " + chosenPrevStation.NextStation;
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




        // Schedule delete or add methods:

        private void lvScheduleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gAddDeparture.Visibility = Visibility.Collapsed;
            bDeleteDeparture.IsEnabled = true;
        }

        private void Button_AddDeparture(object sender, RoutedEventArgs e)
        {
            if (gAddDeparture.Visibility != Visibility.Visible)
                gAddDeparture.Visibility = Visibility.Visible;
            else
                try
                {
                    if (TimeSpan.TryParse(tbAddDeparture.GetLineText(0), out TimeSpan timeDeparture))
                    {
                        bl.AddLineDeparture(timeDeparture, busLine.BusLineID);
                        BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID);
                        gAddDeparture.Visibility = Visibility.Collapsed;
                        tbAddDeparture.Text = "hh:mm:ss";
                    }
                    else
                        MessageBox.Show("You have entered a wrong time departure!");
                }
                catch (ExceptionBL_KeyAlreadyExist)
                {
                    MessageBox.Show("This time departure already exist!");
                }
        }

        private void Button_DeleteDeparture(object sender, RoutedEventArgs e)
        {
            try
            {
                gAddDeparture.Visibility = Visibility.Collapsed;
                bl.DeleteLineDeparture((TimeSpan)lvSchedule.SelectedValue, busLine.BusLineID);
                BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID);
            }
            catch (BO.ExceptionBL_KeyNotFound ex)
            {
                MessageBox.Show("This time departure doesn't exist!");
            }
            catch (BO.ExceptionBL_Inactive ex)
            {
                MessageBox.Show("This time departure already is inactive!");
            }
        }

        // Input validations:

        private void NumberValidationTextBoxNoDots(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

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


    }
}
