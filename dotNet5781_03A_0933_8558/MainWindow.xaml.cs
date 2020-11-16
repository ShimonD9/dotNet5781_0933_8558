using dotNet5781_02_0933_8558;
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

namespace dotNet5781_03A_0933_8558
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Random rnd = new Random(DateTime.Now.Millisecond);
        public static BusLinesCollection busCompany = new BusLinesCollection();
        public static List<BusStop> busStops = new List<BusStop>();
        private BusLine currentDisplayBusLine;

        public MainWindow()
        {
            BusCompanyInitializer(ref busCompany, ref busStops); // Calling the 40 bus stops and 10 bus lines randomizer routine:
            InitializeComponent();
            cbBusLines.ItemsSource = busCompany;
            cbBusLines.DisplayMemberPath = "BusLineNumber";
            tbArea.IsReadOnly = true;
            cbBusLines.SelectedIndex = 0;
        }

        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).BusLineNumber);
        }

        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = busCompany[index];
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStations.DataContext = currentDisplayBusLine.BusStationsList;
        }

        /// <summary>
        /// Bus company initializer, called at the beginning of the program
        /// </summary>
        /// <param name="busCompany"></param>
        /// <param name="busStops"></param>
        public static void BusCompanyInitializer(ref BusLinesCollection busCompany, ref List<BusStop> busStops)
        {
            // The strings are used to create an address
            var bigChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var smallChars = "abcdefghijklmnopqrstuvwxyz";
            var numbers = "0123456789";
            // Creates 40 bus stops
            for (int i = 0; i < 40; ++i)
            {
                char[] stringChars = new char[8];
                // Cutts the strings above to create an address in the form of Abcde 23
                for (int j = 0; j < stringChars.Length; j++)
                {
                    if (j == 0) stringChars[j] = bigChars[rnd.Next(bigChars.Length - 1)];
                    else if (j == 5) stringChars[j] = ' ';
                    else if (j < 5) stringChars[j] = smallChars[rnd.Next(smallChars.Length - 1)];
                    else stringChars[j] = numbers[rnd.Next(numbers.Length - 1)];
                }
                string address = new string(stringChars);
                // Random number for the key of bus stop:
                int stationKey = rnd.Next(100000);
                while (busStops.Any(item => item.BusStopKey == stationKey)) // This loop makes sures there is no duplicateded station key
                {
                    stationKey = rnd.Next(100000);
                }
                BusStop newStation = new BusStop(stationKey, address); // Builds the bus stop
                busStops.Insert(0, newStation);                         // Inserts it to the list
            } // Ending this loop, 40 bus station are initialized

            // Bus lines collection routine:



            for (int i = 0; i < 10; i++)
            {
                int busNumber = i * 100 + 1;
                int area = rnd.Next(0, 5);

                // First station build:

                BusLineStation first = new BusLineStation(busStops[i], 0, 0);

                // Last station build:
                double minutes = Math.Round(20 * rnd.NextDouble() + 1, 1); // Rounding up a randomized double for the minutes, later changed to a time format
                // Assuming the bus travels 1.5 km at a minute, so there will be a logic connection between the minutes and the distance
                BusLineStation last = new BusLineStation(busStops[i + 10], 1.5 * minutes, minutes);

                BusLine newBusLine = new BusLine(busNumber, first, last, area);
                busCompany.busLinesList.Add(newBusLine);

                //  Middle stations build:

                minutes = 20 * rnd.NextDouble() + 1;
                BusLineStation middle = new BusLineStation(busStops[i + 20], 1.5 * minutes, minutes);
                minutes = 20 * rnd.NextDouble() + 1;
                busCompany.busLinesList[i].AddBusStation(middle, first.BusStopKey, 1.5 * minutes, minutes);

                minutes = 20 * rnd.NextDouble() + 1;
                BusLineStation secondMiddle = new BusLineStation(busStops[i + 30], 1.5 * minutes, minutes);
                minutes = 20 * rnd.NextDouble() + 1;
                busCompany.busLinesList[i].AddBusStation(secondMiddle, first.BusStopKey, 1.5 * minutes, minutes);
            } // Ending this loop, we initialized 10 bus lines, using 40 bus stations

            for (int i = 0; i < 10; i++)
            {
                BusLineStation first = new BusLineStation(busStops[i + 1], 0, 0);
                double minutes = Math.Round(20 * rnd.NextDouble() + 1, 1);
                busCompany.busLinesList[i].AddBusStation(first, 0, 1.5 * minutes, minutes);
            } // This loop makes sure that at least 10 bus stations will be used for two different bus lines
        }

        private void tbArea_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
