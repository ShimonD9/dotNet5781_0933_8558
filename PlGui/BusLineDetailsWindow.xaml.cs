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

        public BusLineDetailsWindow()
        {
            InitializeComponent();
        }

        // A second builder, to get the item selected in the list box
        public BusLineDetailsWindow(object item)
        {
            InitializeComponent();
            BusLineDet.DataContext = item;
            busLine = item as BusLine;
        }



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

        private void Button_AddStation(object sender, RoutedEventArgs e)
        {

        }

        private void Button_DeleteStation(object sender, RoutedEventArgs e)
        {
            BO.BusLineStation chosenStation = (lvLineStations.SelectedValue as BusLineStation);
            if (tbDeleteStation.Text == "Delete")
            {
                try
                {
                    if (chosenStation != null && chosenStation.PrevStation != 0 && chosenStation.NextStation != 0)
                    {
                        if (!bl.IsConsecutiveExist(chosenStation.PrevStation, chosenStation.NextStation))
                        {
                            gUpdateConsecutive.Visibility = Visibility.Visible;
                            tbUpdateKmB.Visibility = Visibility.Hidden;
                            tbUpdateTimeB.Visibility = Visibility.Hidden;
                            tbUpdateKM.Text = "0";
                            tbUpdateTime.Text = "hh:mm:ss";
                            lbGapA.Content = chosenStation.PrevStation + " <-> " + chosenStation.NextStation;
                            tbDeleteStation.Text = "Submit changes";
                        }
                        else
                        {
                            bl.DeleteBusLineStation(busLine.BusLineID, chosenStation.BusStopKey, TimeSpan.FromMinutes(0), 0);
                            BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID);
                            gUpdateConsecutive.Visibility = Visibility.Collapsed;
                            bDeleteStation.IsEnabled = false;
                            tbDeleteStation.Text = "Delete";
                        }
                    }
                    else if (chosenStation.PrevStation == 0 || chosenStation.NextStation == 0)
                    {
                        bl.DeleteBusLineStation(busLine.BusLineID, chosenStation.BusStopKey, TimeSpan.FromMinutes(0), 0);
                        BusLineDet.DataContext = bl.GetBusLine(busLine.BusLineID);
                        gUpdateConsecutive.Visibility = Visibility.Collapsed;
                        bDeleteStation.IsEnabled = false;
                        tbDeleteStation.Text = "Delete";
                    }
                }
                catch (BO.ExceptionBL_LessThanTwoStation)
                {
                    MessageBox.Show("There are only two station in the line", "Cannot delete station", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("You didn't fill correctly all the required information", "Cannot add submit the changes", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void lvStationsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gUpdateConsecutive.Visibility = Visibility.Collapsed;
            tbDeleteStation.Text = "Delete";
            bDeleteStation.IsEnabled = true;
        }

        private void lvScheduleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gAddDeparture.Visibility = Visibility.Collapsed;
            bDeleteDeparture.IsEnabled = true;
        }

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
