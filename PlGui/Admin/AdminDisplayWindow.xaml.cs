using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLApi;
using BO;


namespace PlGui
{
    /// <summary>
    /// Interaction logic for AdminDisplayWindow.xaml
    /// </summary>
    public partial class AdminDisplayWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");// Calls and stores the instance of the bl interface
        BO.User admin;

        /// <summary>
        /// Default window ctor
        /// </summary>
        public AdminDisplayWindow()
        {
            InitializeComponent();
            lbBuses.DataContext = bl.GetAllBuses();
            lbBusStops.DataContext = bl.GetAllBusStops();
            lbBusLines.DataContext = bl.GetAllBusLines();
        }


        /// <summary>
        /// Window ctor which get and displays Admin's user name (a feature)
        /// </summary>
        /// <param name="user"></param>
        public AdminDisplayWindow(BO.User user)
        {
            InitializeComponent();
            admin = user;
            string hello = "Hello, ";
            adminWindow.Title = hello + admin.UserName;
            lbBuses.DataContext = bl.GetAllBuses();
            lbBusStops.DataContext = bl.GetAllBusStops();
            lbBusLines.DataContext = bl.GetAllBusLines();
        }


        #region Buses
        /// <summary>
        /// The list view of buses double click - opens the bus details window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LBBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<BusDetailsWindow>().Any())
            {
                ListBox list = sender as ListBox;
                if (list != null)
                {
                    object item = list.SelectedItem; // Gets the selected item, and sends it to the new window builder
                    BusDetailsWindow busDetailsWindow = new BusDetailsWindow(item);
                    busDetailsWindow.ShowDialog();
                    lbBuses.ItemsSource = bl.GetAllBuses(); // Updates the list view

                }
            }
        }

        /// <summary>
        /// The add bus button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenAddBusWindow(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<AddBusWindow>().Any()) // To prevent the openning of another same window
            {
                AddBusWindow addBusWindow = new AddBusWindow(); // Creates the new window, and then shows it
                addBusWindow.ShowDialog();
                lbBuses.ItemsSource = bl.GetAllBuses();
            }
        }



        /// <summary>
        /// The bus refuel button  click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bRefuel_Click(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            if (bus.Fuel == 1200) // In case doesn't need refuel, pops a message
            {
                MessageBox.Show("The gas tank is already full!", "Unable to fill", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                bus.Fuel = 1200;
                bl.UpdateBus(bus); // Calls the bl.update function
                lbBusStops.ItemsSource = bl.GetAllBusStops(); // Updates the list items
                lbBuses.Items.Refresh();
            }
        }


        /// <summary>
        /// The bus treatment button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bTreat_Click(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            if (bus.Mileage - bus.MileageAtLastTreat < 20000 && bus.LastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) > 0) // In case the bus doesn't need a treatment (according to mileage and date of last treatment)
            {
                MessageBox.Show("The bus doesn't need a treatment yet", "No need to treat!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                bus.MileageAtLastTreat = bus.Mileage;
                bus.LastTreatmentDate = DateTime.Now;
                bl.UpdateBus(bus); // Calls the bl.update function
                lbBusStops.ItemsSource = bl.GetAllBusStops(); // Updates the list items
                lbBuses.Items.Refresh();
            }
        }
        #endregion

        #region Bus Lines
        /// <summary>
        /// The list view of bus lines double click - opens the bus details window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LBBusLines_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<BusLineDetailsWindow>().Any())
            {
                ListBox list = sender as ListBox;
                if (list != null)
                {
                    object item = list.SelectedItem; // Gets the selected item, and sends it to the new window builder
                    BusLineDetailsWindow busLineDetailsWindow = new BusLineDetailsWindow(item);
                    busLineDetailsWindow.ShowDialog();
                    lbBusLines.ItemsSource = bl.GetAllBusLines();  // Updates the list view
                }
            }
        }

        /// <summary>
        /// The add bus line button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenAddBusLineWindow(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<AddBusLineWindow>().Any()) // To prevent the openning of another same window
            {
                AddBusLineWindow addBusLineWindow = new AddBusLineWindow(); // Creates the new window, and then shows it
                addBusLineWindow.ShowDialog();
                lbBusLines.ItemsSource = bl.GetAllBusLines();
            }
        }
        #endregion

        #region Bus Stops
        /// <summary>
        /// The list view of bus stops double click - opens the bus details window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LBBusStops_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<BusStopDetailsWindow>().Any())
            {
                ListBox list = sender as ListBox;
                if (list != null)
                {
                    object item = list.SelectedItem; // Gets the selected item, and sends it to the new window builder
                    BusStopDetailsWindow busStopDetailsWindow = new BusStopDetailsWindow(item);
                    busStopDetailsWindow.ShowDialog();
                    lbBusStops.ItemsSource = bl.GetAllBusStops();  // Updates the list view
                }
            }
        }

        /// <summary>
        /// The add bus stop button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenAddBusStopWindow(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<AddBusStopWindow>().Any()) // To prevent the openning of another same window
            {
                AddBusStopWindow addBusStopWindow = new AddBusStopWindow(); // Creates the new window, and then shows it
                addBusStopWindow.ShowDialog();
                lbBusStops.ItemsSource = bl.GetAllBusStops();
            }
        }
        #endregion

        /// <summary>
        /// Menu item exit option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Back to login window click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); // Creates the new window, and then shows it
            mainWindow.Show();
            this.Close();
        }
    }
}
