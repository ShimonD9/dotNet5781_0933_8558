using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
using System.Text.RegularExpressions;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for BusDetailsWindow.xaml
    /// </summary>
    public partial class BusDetailsWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");
        BO.Bus bus;

        /// <summary>
        /// Default window ctor
        /// </summary>
        public BusDetailsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Second window ctor recieving the bus details double-clicked item
        /// </summary>
        /// <param name="item"></param>
        public BusDetailsWindow(object item)
        {
            InitializeComponent();
            bus = item as BO.Bus;
            BusDet.DataContext = item;
        }


        /// <summary>
        /// The update button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Update(object sender, RoutedEventArgs e)
        {
            // Chosen dates (of license and last treatment) declared:
            DateTime startDateChosen;
            DateTime treatDateChosen;
            try
            {
                // Checks if the user chose a date
                if (!dpLicenseDate.SelectedDate.HasValue || !dpTreatmentDate.SelectedDate.HasValue) // Checks if the user chose a date
                {
                    MessageBox.Show("You didn't fill the required date fields!", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // The dates initialized:
                    startDateChosen = dpLicenseDate.SelectedDate.Value;
                    treatDateChosen = dpTreatmentDate.SelectedDate.Value;

                    // Checks if the inputs are correct, and pops an appropriate message if not (not made in BL because the connection to the text box length and the double parse of string)                try
                    {
                        if (startDateChosen.Year < 2018 && tbLicense.Text.Length < 7
                        || startDateChosen.Year > 2017 && tbLicense.Text.Length < 8)
                        {
                            MessageBox.Show("The license you entered is too short!", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else if (!Double.TryParse(tbMileage.GetLineText(0), out double milNow) || !Double.TryParse(tbMileageAtTreat.GetLineText(0), out double milTreat))
                        {
                            MessageBox.Show("You didn't fill correctly all the required information", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            // Initializes the new bus properties:
                            bus.Fuel = sliderFuel.Value;
                            bus.LicenseDate = startDateChosen;
                            bus.LastTreatmentDate = treatDateChosen;
                            bus.Mileage = double.Parse(tbMileage.Text);
                            bus.MileageAtLastTreat = double.Parse(tbMileageAtTreat.Text);
                            bl.UpdateBus(bus);
                            this.Close(); // Closes the window
                        }
                    }

                }
            }
            catch (BO.ExceptionBL_KeyNotFound) // In case the bus doesn't exist or has been deactivated
            {
                MessageBox.Show("License does not exist or bus inactive", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (BO.ExceptionBL_MileageValuesConflict) // In case there is a logical conflict between the two mileages entered
            {
                MessageBox.Show("The total mileage cannot be smaller than the mileage at the last treat!", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this bus?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bl.DeleteBus(bus.License); // Calls the bl.DeleteBus function
                }
                catch (BO.ExceptionBL_KeyNotFound) // Catchs and prints message if the bus wasn't found
                {
                    MessageBox.Show("The bus license doesn't exist or the bus is inactive!", "Cannot delete the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                this.Close(); // Closes the window
            }
        }


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

    }
}