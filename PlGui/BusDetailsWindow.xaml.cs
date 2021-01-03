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

namespace PlGui
{
    /// <summary>
    /// Interaction logic for BusDetailsWindow.xaml
    /// </summary>
    public partial class BusDetailsWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");
        BO.Bus bus;


        //public BusDetailsWindow()
        //{
        //    InitializeComponent();
        //}

        // A second builder, to get the item selected in the list box
        public BusDetailsWindow(object item)
        {
            InitializeComponent();
            bus = item as BO.Bus;
            BusDet.DataContext = item;
        }


        /// <summary>
        /// The refuel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Update(object sender, RoutedEventArgs e)
        {
            DateTime startDateChosen;
            DateTime treatDateChosen;
            if (!dpLicenseDate.SelectedDate.HasValue || !dpTreatmentDate.SelectedDate.HasValue) // Checks if the user chose a date
            {
                MessageBox.Show("You didn't fill the required date fields!", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                startDateChosen = dpLicenseDate.SelectedDate.Value;
                treatDateChosen = dpTreatmentDate.SelectedDate.Value;
                // Checks if the inputs are correct, and pops an appropriate message if not:
                try
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
                    else if (double.Parse(tbMileageAtTreat.Text) > double.Parse(tbMileage.Text))
                    {
                        MessageBox.Show("The total mileage cannot be smaller than the mileage at the last treat!", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        bus.Fuel = sliderFuel.Value;
                        bus.LicenseDate = startDateChosen;
                        bus.LastTreatmentDate = treatDateChosen;
                        bus.Mileage = double.Parse(tbMileage.Text);
                        bus.MileageAtLastTreat = double.Parse(tbMileageAtTreat.Text);
                        bl.UpdateBus(bus);                        
                        this.Close(); // Closes the window
                    }
                }
                catch (BO.ExceptionBLBadLicense)
                {
                    MessageBox.Show("The bus license you entered already exists in the company!", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// The delete button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this bus?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bl.DeleteBus(bus.License);
                }
                catch (BO.ExceptionBLBadLicense)
                {
                    MessageBox.Show("The bus license doesn't exist or the bus is inactive!", "Cannot delete the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                this.Close(); // Closes the window
            }
        }
    }
}