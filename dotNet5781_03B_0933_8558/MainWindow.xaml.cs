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
using System.Threading;

namespace dotNet5781_03B_0933_8558
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged // Inotify - for the clock simulator property changes
    {

        public static List<Bus> busList = new List<Bus> { };

        public MainWindow()
        {
            InitializeComponent();
            BusesInitializer(ref busList); // The bus list is being initialized
            lbBuses.DataContext = busList; // The list view data context is initialized with the bus list
        }

        /// <summary>
        /// The window loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RunClock(); // Calls the similator clock background worker
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
                lbBuses.Items.Refresh(); // For seeing the new bus added on the list view
            }
        }

        /// <summary>
        /// The refuel bus button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_RefuelTheBus(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            if (bus.KMLeftToTravel == 1200) // In case doesn't need refuel, pops a message
            {
                MessageBox.Show("The gas tank is already full!", "You are a loser", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                bus.Refuel(); // Calls the background worker to refuel the bus (the worker is in bus.cs)
            }
        }

        /// <summary>
        /// The pick up bus for a ride button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenPickUpBusWindow(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            if (!Application.Current.Windows.OfType<PickUpBusWindow>().Any()) // To prevent the openning of another same window
            {
                PickUpBusWindow pickUpBusWindow = new PickUpBusWindow(bus); // Creates the new window, and then shows it
                pickUpBusWindow.ShowDialog();
            }
        }

        /// <summary>
        /// The double click on list view for details event
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
                    busDetailsWindow.Show();
                }
            }
        }


        public static Random rnd = new Random(DateTime.Now.Millisecond); // For creating random buses
        public static void BusesInitializer(ref List<Bus> busList)
        {
            bool flag;                  //for checking if there is two buses with the same license
            string license = null;
            // Before 2018:
            for (int i = 0; i < 5; i++)                     //add 5 buses befor 2018
            {
                var numbers = "1234567890";                 //numbers to randomise for the license
                char[] stringChars = new char[7];   
                stringChars[0] = numbers[rnd.Next(numbers.Length - 1)]; // To avoid 0 at the beginning of the license
                flag = true;
                while (flag)  // Makes sure there are no two buses with same license number
                {
                    for (int j = 1; j < stringChars.Length; j++)    //loop for taking each number from the string separately
                    {
                        stringChars[j] = numbers[rnd.Next(numbers.Length)];     //randomise number
                        license = new string(stringChars);                      //create the license
                    }
                    flag = FindIfBusExist(busList, license);                    //check if there is already a bus woth the same number
                }

                var year = rnd.Next(1990, 2018);                                    //random year
                var month = rnd.Next(1, 13);                                        //random month
                var days = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);      //random day
                DateTime absorptionDate = new DateTime(year, month, days);          //create the starting date of the bus
                //randomise the km for travel ,for traetment ect
                double km = Math.Round(rnd.NextDouble() * 50000 + 20000, 2);        // Minimum of 20,000 mileage, maximum of 70,000
                double kmAtLastTreatment = Math.Round(km - rnd.NextDouble() * 20000, 2); // Minimum of 0, maximum of 70,000, respectively
                Bus newBus = new Bus(license, km, kmAtLastTreatment, absorptionDate, DateTime.Now.AddDays(-1 * rnd.Next(1, 350)).Date);

                busList.Add(newBus);        //add the bus
            }
            busList[4].LastTreatmentDate = DateTime.Now.AddDays(-366).Date; // One of the buses should be after the needed treatment (= so the bus is dangerous to travel)


            // After 2018:
            for (int i = 0; i < 5; i++)                 //add 5 buses after 2018
            {
                var numbers = "1234567890";             //numbers to randomise for the license
                char[] stringChars = new char[8];
                stringChars[0] = numbers[rnd.Next(numbers.Length - 1)]; // To avoid 0 at the beginning of the license
                flag = true;
                while (flag)  // Makes sure there are no two buses with same license number
                {
                    for (int j = 1; j < stringChars.Length; j++)         //loop for taking each number from the string separately
                    {
                        stringChars[j] = numbers[rnd.Next(numbers.Length)];     //taking one number every each round
                        license = new string(stringChars);                      //create the license
                    }
                    flag = FindIfBusExist(busList, license);
                }

                //randomise date for the bus
                var year = rnd.Next(2018, 2022);
                var month = rnd.Next(1, 13);
                var days = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);
                DateTime absorptionDate = new DateTime(year, month, days);
                //randomise the km for travel ,for traetment ect
                double km = Math.Round(rnd.NextDouble() * 50000 + 20000, 2); // Minimum of 20,000 mileage, maximum of 70,000
                double kmAtLastTreatment = Math.Round(km - rnd.NextDouble() * 20000, 2); // Minimum of 0, maximum of 70,000, respectively
                Bus newBus = new Bus(license, km, kmAtLastTreatment, absorptionDate, DateTime.Now.AddDays(-1 * rnd.Next(1, 350)).Date);

                busList.Add(newBus);    //add the bus
            }
            busList[8].KMLeftToTravel = 50; // A bus with small amount of fuel 
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
                if (bus.CompareLicenses(license))       //in case two licens with the same number was found
                {
                    return true;
                }
            }
            return false;
        }


        //////////////////////////////////////////////// CLOCK ////////////////////////////////////////////////
        
        // For updating the simulator clock on the GUI we used the PropertyChanged method
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        // The field created to be shared on other parts of the project
        public static DateTime useMyRunningDate;

        // The field of runningDate first initialized
        private DateTime runningDate = DateTime.Now;
        
        // The property of the field
        public DateTime RunningDate
        {
            get { return runningDate; }
            set { runningDate = value; useMyRunningDate = value; OnPropertyChanged("RunningDate"); }
        }

        // The buses days left to treatment && status updated in this method
        public static void updateBuses(DateTime RunningDate)
        {
            foreach (Bus b in busList)
            {
                DateTime dateOnly = new DateTime(RunningDate.Year, RunningDate.Month, RunningDate.Day);
                b.DaysUntilNextTreat = (b.LastTreatmentDate.AddYears(1) - dateOnly).Days; // Updates the days to treatment each new day
                if (b.Status == Bus.BUS_STATUS.READY_FOR_TRAVEL || b.Status == Bus.BUS_STATUS.NEEDS_REFUEL) // The status is being updated to dangerous only in those cases (if the days left are 0 or less)
                    b.Update_Status(); 
            }
        }

        public static bool shouldStop = false;

        // The background worker for the clock:
        private readonly BackgroundWorker clockWorker = new BackgroundWorker();

        public void RunClock()
        {

            clockWorker.WorkerReportsProgress = true;

            clockWorker.ProgressChanged += (sender, args) =>
            {
                RunningDate = RunningDate.AddMinutes(1); // Updates the minutes (each minutes = 0.1 seconds in real time)
                updateBuses(RunningDate);
            };

            clockWorker.DoWork += (sender, args) =>
            {
                if (clockWorker.CancellationPending == true)
                {

                }
                else
                {
                    while (shouldStop == false)
                    {
                        try { Thread.Sleep(100); } catch (Exception) { }
                        clockWorker.ReportProgress(0);
                    }
                }
            };

            if (clockWorker.IsBusy != true)
                clockWorker.RunWorkerAsync();

            clockWorker.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Cancelled == true)
                {

                }
                else
                {

                }
            };
        }

    }
}
