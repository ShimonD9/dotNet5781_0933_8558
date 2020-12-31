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
using BLApi;
using BO;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for BusStopDetailsWindowWindow.xaml
    /// </summary>
    public partial class BusStopDetailsWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");
        BO.BusStop busStop;
        public BusStopDetailsWindow()
        {
            InitializeComponent();
        }

        // A second builder, to get the item selected in the list box
        public BusStopDetailsWindow(object item)
        {
            InitializeComponent();
            BusStopDet.DataContext = item;
            busStop = item as BO.BusStop;
        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this bus stop?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bl.DeleteBusStop(busStop.BusStopKey);
                    this.Close(); // Closes the window
                }
                catch (BO.ExceptionBLBadLicense)
                {
                    MessageBox.Show("The bus license doesn't exist or the bus is inactive!", "Cannot delete the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch
                {
                    string busLines = string.Join(", ", from lineBus in busStop.LinesStopHere select lineBus.BusLineNumber);
                    MessageBox.Show("This bus stop serves the next bus lines: \n" + busLines + ".\nYou must remove the bus station from the bus lines details window, before deleting the bus stop itself.", "Unable to delte the bus stop!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
