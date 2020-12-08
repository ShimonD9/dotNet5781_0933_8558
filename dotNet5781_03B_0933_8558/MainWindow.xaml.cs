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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

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


        private void Button_RefuelTheBus(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            if (bus.KMLeftToTravel == 1200)
            {
                MessageBox.Show("The gas tank is already full!", "You are a loser", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                bus.Refuel();
            }
        }

        private void Button_OpenPickUpBusWindow(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            Bus bus = fxElt.DataContext as Bus;
            if (!Application.Current.Windows.OfType<PickUpBusWindow>().Any())
            {
                PickUpBusWindow pickUpBusWindow = new PickUpBusWindow(bus);
                pickUpBusWindow.ShowDialog();
            }
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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        private DateTime runningDate = DateTime.Now;
        public static DateTime useMyRunningDate;
        public DateTime RunningDate
        {
            get { return runningDate; }
            set { runningDate = value; useMyRunningDate = value; OnPropertyChanged("RunningDate"); }
        }

        public static void updateBuses(DateTime RunningDate)
        {
            foreach (Bus b in busList)
            {
                DateTime dateOnly = new DateTime(RunningDate.Year, RunningDate.Month, RunningDate.Day);
                b.DaysUntilNextTreat = (b.LastTreatmentDate.AddYears(1) - dateOnly).Days;
                if (b.Status == Bus.BUS_STATUS.READY_FOR_TRAVEL || b.Status == Bus.BUS_STATUS.NEEDS_REFUEL)
                    b.Update_Status();
            }
        }

        public static bool shouldStop = false;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RunClock();
        }

        private readonly BackgroundWorker clockWorker = new BackgroundWorker();

        public void RunClock()
        {

            clockWorker.WorkerReportsProgress = true;

            clockWorker.ProgressChanged += (sender, args) =>
            {
                RunningDate = RunningDate.AddMinutes(1);
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
