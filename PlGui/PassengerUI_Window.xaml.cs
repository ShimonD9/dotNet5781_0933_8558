using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    /// Interaction logic for PassengerUI_Window.xaml
    /// </summary>
    public partial class PassengerUI_Window : Window, INotifyPropertyChanged // Inotify - for the clock simulator property changes
    {
        IBL bl = BLFactory.GetBL("1"); // Calls and stores the instance of the bl interface
        User passenger; // Creates a BO.passenger for title greetings feature
        BusStop busStop; // Creates a BO.busStop 


        /// <summary>
        /// Default window ctor
        /// </summary>
        public PassengerUI_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Window ctor with user from the main window for the greetings title feature
        /// </summary>
        /// <param name="user"></param>
        public PassengerUI_Window(BO.User user)
        {
            InitializeComponent();
            passenger = user;
            changeTitleAsDayTime(); // Calls the title greeting function
            timeEdit.Text = DateTime.Now.ToString("HH:mm:ss"); // The time edit text will hold the current time of clock
            cbBusStop.ItemsSource = bl.GetAllBusStops().OrderBy(busStop => busStop.BusStopKey); // The bus stops combo box will hold the bus stops by order of their key
        }

        /// <summary>
        /// Bus stop selection changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbBusStop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            intervalSlider.IsEnabled = true;
            timeEdit.IsEnabled = true;
            Start_Pause.IsEnabled = true; // After choosing the bus stop, the user is able to start the clock simulator
            tbNoBuses.Visibility = Visibility.Collapsed; // The no buses info label collapsing

            busStop = cbBusStop.SelectedItem as BO.BusStop;
            lvLinesStopHere.DataContext = busStop; // The list view data context is updated

            // The digital panel item source is being initialized by bl.GetLineTimingsPerStation
            var linesForDigitalPanelCollection = bl.GetLineTimingsPerStation(busStop, runningTime);
            if (linesForDigitalPanelCollection.Count() == 0)
                tbNoBuses.Visibility = Visibility.Visible; // In case there are no close bus lines, the info label will be visible
            lvMinutesToBus.ItemsSource = linesForDigitalPanelCollection;
        }


        /// <summary>
        /// The title greetings feature
        /// </summary>
        private void changeTitleAsDayTime()
        {

            string time = "Hello, ";
            int hour = RunningTime.Hours;
            if (hour > 5 && hour < 12)
                time = "Good morning, ";
            else if (hour >= 12 && hour < 17)
                time = "Good afternoon, ";
            else if (hour >= 17 && hour < 20)
                time = "Good evening, ";
            else if (hour >= 20 || hour < 5)
                time = "Good night, ";
            PassengerWindow.Title = time + passenger.UserName;
        }


        /// <summary>
        /// For input preview validation in the text box of time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberValidationTextBoxColon(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/:]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Logout mouse down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); // Creates the new window, and then shows it
            mainWindow.Show();
            this.Close();
        }

        #region Clock simulator


        private int secondsInterval; // Holds the interval integer
        private static bool shouldStop = true; // Tells if the background worker loop should stop
        private static bool everStarted = false; // In case the background worker never has been started
        private static bool isStarted = false; // To know if the button is at start or pause

        /// <summary>
        /// Button start/pause click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void start_Pause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isStarted) // To know if the proccess at work
                {
                    if (!TimeSpan.TryParse(timeEdit.Text, out TimeSpan inputTime)) // Try parses the text box
                        MessageBox.Show("Wrong time input!");
                    else
                    {

                        shouldStop = false; // It means the work should continue (at the background worker)
                        isStarted = true; // For the button condition
                       
                        secondsInterval = (int)intervalSlider.Value;  // Updates the seconds interval
                        RunningTime = inputTime; // Updates the running time variable
                        
                        if (!everStarted) // Runs the clock for the first time the button clicked
                        {
                            runClock(); // Calls the function which creates the background worker at the first place
                            everStarted = true; 
                        }
                        else if (!clockWorker.IsBusy)
                            clockWorker.RunWorkerAsync(); // Continues the sync from the current backgroundworker

                        // Text, buttons and visibility updates:
                        tbStart_Pause.Text = "Pause"; 
                        timeDisplay.Visibility = Visibility.Visible;
                        timeEdit.Visibility = Visibility.Collapsed;
                        intervalSlider.IsEnabled = false;
                        cbBusStop.IsEnabled = false;
                    }
                }
                else // If the user wants to stop the running clock
                {
                    if (clockWorker.IsBusy)
                    {
                        clockWorker.CancelAsync(); // Calls cancellation if the worker is busy
                    }

                    // Bool values update:
                    shouldStop = true;
                    isStarted = false;

                    // Text, buttons and visibility updates:
                    timeEdit.Text = RunningTime.ToString();
                    tbStart_Pause.Text = "Start";
                    timeDisplay.Visibility = Visibility.Collapsed;
                    timeEdit.Visibility = Visibility.Visible;
                    intervalSlider.IsEnabled = true;
                    cbBusStop.IsEnabled = true;
                }
            }
            catch // For unexpected issues
            {
                MessageBox.Show("An unexpected problem occured", "ERROR");
            }
        }


        // For updating the simulator clock on the GUI we used the PropertyChanged method
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }



        // The field of runningDate first initialized
        private TimeSpan runningTime = DateTime.Now.TimeOfDay;

        // The property of the field
        public TimeSpan RunningTime
        {
            get { return runningTime; }
            set { runningTime = value; OnPropertyChanged("RunningTime"); }
        }


        // The background worker for the clock:
        private readonly BackgroundWorker clockWorker = new BackgroundWorker();

        /// <summary>
        /// The runClock background worker function
        /// </summary>
        private void runClock()
        {
            int t = 5; // Meant for the digital panel update loop (every 5 seconds real time)
            clockWorker.WorkerReportsProgress = true; 
            clockWorker.WorkerSupportsCancellation = true;

            // The progress changed update:
            clockWorker.ProgressChanged += (sender, args) =>
            {
                // Running time and display update:
                RunningTime = RunningTime.Add(TimeSpan.FromSeconds(secondsInterval));
                if (RunningTime.Days > 0) RunningTime = RunningTime.Subtract(TimeSpan.FromDays(RunningTime.Days)); // To not show any days
                timeDisplay.Text = RunningTime.ToString();

                if (t == 5) 
                {
                    changeTitleAsDayTime(); // Window title update (just a boring feature)
                    var minutesToBus = bl.GetLineTimingsPerStation(busStop, RunningTime); // Calls the bl to get the line timings for the current bus stop and time
                    if (minutesToBus.Count() == 0) // If there are no line timings, it makes the text box of no buses visible
                        tbNoBuses.Visibility = Visibility.Visible;
                    else
                        tbNoBuses.Visibility = Visibility.Collapsed;
                    lvMinutesToBus.ItemsSource = minutesToBus; // Updates the list view with the collection
                    t = 0; // For the digital panel be updated again in 5 seconds
                }
                t++;
            };

            clockWorker.DoWork += (sender, args) => // Adding a new clock worker
            {
                if (clockWorker.CancellationPending == true)
                {
                    args.Cancel = true; // To cancel the work
                }
                else
                {
                    while (shouldStop == false) // While at work and shouldn't stop, it will update proccess each seconds
                    {
                        try { clockWorker.ReportProgress(0); Thread.Sleep(1000); } catch (Exception) { }
                    }
                }
            };

            if (clockWorker.IsBusy != true) // Will run if it isn't busy
            {
                clockWorker.RunWorkerAsync();
            }


            clockWorker.RunWorkerCompleted += (sender, args) => // For the complition (nothing needed to be done, but for the correctnes of the backgroundworker I didn't touch it)
            {
                if (args.Cancelled == true)
                {
                    // No need to update the user
                }
                else
                {
                    // No need to update the user
                }
            };
        }

        /// <summary>
        /// The window closing event, forces the background worker to stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (clockWorker.IsBusy)
            {
                clockWorker.CancelAsync();
            }
        }


        #endregion

    }
}
