using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for BusDetailsWindow.xaml
    /// </summary>
    public partial class BusDetailsWindow : Window
    {
        public BusDetailsWindow()
        {
            InitializeComponent();
        }

        // A second builder, to get the item selected in the list box
        public BusDetailsWindow(object item)
        {
            InitializeComponent();
            BusDet.DataContext = item;
        }


        /// <summary>
        /// The refuel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Fuel(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
            //var fxElt = sender as FrameworkElement;
            //Bus bus = fxElt.DataContext as Bus;
            //if (bus.KMLeftToTravel == 1200) // In case the gas tank is already full
            //{
            //    MessageBox.Show("The gas tank is already full!", "Refuel Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
            //else
            //{
            //    bus.Refuel(); // Calls the refuel background worker
            //    this.Close(); // Closes the window
            //}
        }

        /// <summary>
        /// The treatment button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Treatment(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
            //var fxElt = sender as FrameworkElement;
            //Bus bus = fxElt.DataContext as Bus;
            //if (bus.MileageSinceLastTreat < 20000 && bus.LastTreatmentDate.AddYears(1).CompareTo(MainWindow.useMyRunningDate) > 0) // In case the bus doesn't need a treatment (according to mileage and date of last treatment)
            //{
            //    MessageBox.Show("The bus doesn't need a treatment yet", "Treatment Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
            //else
            //{
            //    bus.Treatment();  // Calls the refuel background worker
            //    this.Close();  // Closes the window
            //}
        }
    }
}
