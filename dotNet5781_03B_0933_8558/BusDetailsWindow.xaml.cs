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

namespace dotNet5781_03B_0933_8558
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

        public BusDetailsWindow(object item)
        {
            InitializeComponent();
            BusDet.DataContext = item;
        }

        private void Button_Fuel(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            if (bus.KMLeftToTravel == 1200)
            {
                MessageBox.Show("The bus gas tank is already full!", "Refuel Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                bus.Refuel();
                this.Close();
            }
        }

        private void Button_Treatment(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            if (bus.MileageSinceLastTreat < 20000 && bus.LastTreatmentDate.AddYears(1).CompareTo(MainWindow.useMyRunningDate) > 0)
            {
                MessageBox.Show("The bus doesn't need a treatment yet", "Treatment Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }           
            else
            {
                bus.Treatment();
                this.Close();
            }
        }
    }
}
