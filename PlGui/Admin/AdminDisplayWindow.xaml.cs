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
            lvBuses.DataContext = bl.GetAllBuses();
            lvBusStops.DataContext = bl.GetAllBusStops();
            lvBusLines.DataContext = bl.GetAllBusLines();
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
            lvBuses.DataContext = bl.GetAllBuses();
            lvBusStops.DataContext = bl.GetAllBusStops();
            lvBusLines.DataContext = bl.GetAllBusLines();
            lvAreas.DataContext = bl.GetLineByArea();
        }


        #region Buses
        /// <summary>
        /// The list view of buses double click - opens the bus details window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lBBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<BusDetailsWindow>().Any())
            {
                ListBox list = sender as ListBox;
                if (list != null)
                {
                    object item = list.SelectedItem; // Gets the selected item, and sends it to the new window builder
                    BusDetailsWindow busDetailsWindow = new BusDetailsWindow(item);
                    busDetailsWindow.ShowDialog();
                    lvBuses.ItemsSource = bl.GetAllBuses(); // Updates the list view

                }
            }
        }

        /// <summary>
        /// The add bus button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_openAddBusWindow(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<AddBusWindow>().Any()) // To prevent the openning of another same window
            {
                AddBusWindow addBusWindow = new AddBusWindow(); // Creates the new window, and then shows it
                addBusWindow.ShowDialog();
                lvBuses.ItemsSource = bl.GetAllBuses();
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
            try 
            { bl.RefuelBus(bus); lvBuses.Items.Refresh(); } // Calls bl.refuel and updates the list items
            catch (BO.ExceptionBL_NoNeedToRefuel) // In case doesn't need refuel, pops a message
            {
                MessageBox.Show("The gas tank is already full!", "Unable to fill", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)// For unexpected issues
            {
                MessageBox.Show("An unexpected problem occured: " + ex.Message, "Unable to fill");
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
            try 
            { bl.TreatBus(bus); lvBuses.Items.Refresh(); } // Calls bl.treat and updates the list items
            catch (BO.ExceptionBL_NoNeedToTreat)
            {
                MessageBox.Show("The bus doesn't need a treatment yet", "No need to treat!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)// For unexpected issues
            {
                MessageBox.Show("An unexpected problem occured: " + ex.Message, "Unable to treat");
            }
        }
        #endregion

        #region Bus Lines
        /// <summary>
        /// The list view of bus lines double click - opens the bus details window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lBBusLines_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<BusLineDetailsWindow>().Any())
            {
                ListBox list = sender as ListBox;
                if (list != null)
                {
                    object item = list.SelectedItem; // Gets the selected item, and sends it to the new window builder
                    BusLineDetailsWindow busLineDetailsWindow = new BusLineDetailsWindow(item);
                    busLineDetailsWindow.ShowDialog();
                    lvBusLines.ItemsSource = bl.GetAllBusLines();  // Updates the list view
                }
            }
        }

        /// <summary>
        /// The add bus line button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OpenAddBusLineWindow(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<AddBusLineWindow>().Any()) // To prevent the openning of another same window
            {
                AddBusLineWindow addBusLineWindow = new AddBusLineWindow(); // Creates the new window, and then shows it
                addBusLineWindow.ShowDialog();
                lvBusLines.ItemsSource = bl.GetAllBusLines();
            }
        }
        #endregion

        #region Bus Stops
        /// <summary>
        /// The list view of bus stops double click - opens the bus details window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lBBusStops_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<BusStopDetailsWindow>().Any())
            {
                ListBox list = sender as ListBox;
                if (list != null)
                {
                    object item = list.SelectedItem; // Gets the selected item, and sends it to the new window builder
                    BusStopDetailsWindow busStopDetailsWindow = new BusStopDetailsWindow(item);
                    busStopDetailsWindow.ShowDialog();
                    lvBusStops.ItemsSource = bl.GetAllBusStops();  // Updates the list view
                }
            }
        }

        /// <summary>
        /// The add bus stop button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OpenAddBusStopWindow(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<AddBusStopWindow>().Any()) // To prevent the openning of another same window
            {
                AddBusStopWindow addBusStopWindow = new AddBusStopWindow(); // Creates the new window, and then shows it
                addBusStopWindow.ShowDialog();
                lvBusStops.ItemsSource = bl.GetAllBusStops();
            }
        }
        #endregion

        /// <summary>
        /// Menu item exit option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Back to login window click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); // Creates the new window, and then shows it
            mainWindow.Show();
            this.Close();
        }
    }
}
