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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace dotNet5781_03B_0933_8558
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //BackgroundWorker refuel = new BackgroundWorker();
        public static Random rnd = new Random(DateTime.Now.Millisecond);
        public static List<Bus> busList = new List<Bus> { };

        public MainWindow()
        {
            BusesInitializer(ref busList);
            InitializeComponent();
            lbBuses.DataContext = busList;
            
            
        }

        private void Button_OpenAddBusWindow(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<AddBusWindow>().Any())
            {
                AddBusWindow addBusWindow = new AddBusWindow();
                addBusWindow.ShowDialog();
                lbBuses.Items.Refresh();
            }
        }

        private void Button_OpenPickUpBusWindow(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            if (bus.Status == Bus.BUS_STATUS.READY_FOR_TRAVEL)
            {
                if (!Application.Current.Windows.OfType<PickUpBusWindow>().Any())
                {
                    PickUpBusWindow pickUpBusWindow = new PickUpBusWindow();
                    pickUpBusWindow.Show();
                }
            }
            else
                MessageBox.Show("The bus cannot travel currently", "You are so funny", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Button_RefuelTheBus(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            if (bus.KMLeftToRide == 1200)
            {
                MessageBox.Show("The bus gas tank is already full!", "You are a loser", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //(sender as Button).IsEnabled = false;
                bus.Refuel();
                //(sender as Button).IsEnabled = true;
            }
            // Background - calling the worker of bus fuel
        }

        private void LBBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<BusDetailsWindow>().Any())
            {
                ListBox list = sender as ListBox;
                if (list != null)
                {
                    object item = list.SelectedItem;
                    BusDetailsWindow busDetailsWindow = new BusDetailsWindow(item);
                    busDetailsWindow.Show();
                }
            }
        }


        public static void BusesInitializer(ref List<Bus> busList)
        {
            bool flag;
            string license = null;
            // Before 2018:
            for (int i = 0; i < 5; i++)
            {
                var numbers = "1234567890";
                char[] stringChars = new char[7];
                stringChars[0] = numbers[rnd.Next(numbers.Length - 1)]; // To avoid 0 at the beginning of the license
                flag = true;
                while (flag)  // Makes sure there are no two buses with same license number
                {
                    for (int j = 1; j < stringChars.Length; j++)
                    {
                        stringChars[j] = numbers[rnd.Next(numbers.Length)];
                        license = new string(stringChars);
                    }
                    flag = FindIfBusExist(busList, license);
                }

                var year = rnd.Next(1990, 2018);
                var month = rnd.Next(1, 13);
                var days = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);
                DateTime absorptionDate = new DateTime(year, month, days);

                double km = Math.Round(rnd.NextDouble() * 200000 + 20000, 2);
                double kmAtLastTreatment = Math.Round(km - rnd.NextDouble() * 10000, 2);
                Bus newBus = new Bus(license, km, kmAtLastTreatment, absorptionDate, DateTime.Now.AddDays(-1 * rnd.Next(1, 200)));

                busList.Add(newBus);
            }
            busList[4].LastTreatmentDate = DateTime.Now.AddDays(-367); // One of the buses should be after the needed treatment (= so the bus is dangerous to travel)


            // After 2018:
            for (int i = 0; i < 5; i++)
            {
                var numbers = "1234567890";
                char[] stringChars = new char[8];
                stringChars[0] = numbers[rnd.Next(numbers.Length - 1)]; // To avoid 0 at the beginning of the license
                flag = true;
                while (flag)  // Makes sure there are no two buses with same license number
                {
                    for (int j = 1; j < stringChars.Length; j++)
                    {
                        stringChars[j] = numbers[rnd.Next(numbers.Length)];
                        license = new string(stringChars);
                    }
                    flag = FindIfBusExist(busList, license);
                }

                var year = rnd.Next(2018, 2022);
                var month = rnd.Next(1, 13);
                var days = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);
                DateTime absorptionDate = new DateTime(year, month, days);

                double km = Math.Round(rnd.NextDouble() * 200000 + 20000, 2);
                double kmAtLastTreatment = Math.Round(km - rnd.NextDouble() * 10000, 2);
                Bus newBus = new Bus(license, km, kmAtLastTreatment, absorptionDate, DateTime.Now.AddDays(-1 * rnd.Next(1, 300)));

                busList.Add(newBus);
            }
            busList[8].KMLeftToRide = 50; // A bus with small amount of fuel 
            busList[9].MileageAtLastTreat = busList[9].Mileage - 19500; // For the bus that needs to make treatment soon because it reaches 20,000 km since last treatment
        }

        /// <summary>
        /// Searchs trough a given list of buses if a bus exist with the given string, and returns true if yes
        /// </summary>
        /// <param name="buses"></param>
        /// <param name="license"></param>
        /// <returns></returns>
        public static bool FindIfBusExist(List<Bus> buses, string license)
        {
            foreach (Bus bus in buses)
            {
                if (bus.CompareLicenses(license))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
