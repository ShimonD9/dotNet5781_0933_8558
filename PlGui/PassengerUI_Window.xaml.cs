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


        bool everStarted = false; // In case the background worker never has been started
        bool isStarted = false; // To know if the button is at start or pause

        private void start_Pause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isStarted)
                {
                    if (!TimeSpan.TryParse(timeEdit.Text, out TimeSpan inputTime))
                        MessageBox.Show("Wrong time input!");
                    else
                    {

                        shouldStop = false;
                        isStarted = true;
                        RunningTime = inputTime;
                        if (!everStarted)
                            runClock();
                        else clockWorker.RunWorkerAsync();
                        everStarted = true;
                        tbStart_Pause.Text = "Pause";
                        timeDisplay.Visibility = Visibility.Visible;
                        timeEdit.Visibility = Visibility.Collapsed;
                        intervalSlider.IsEnabled = false;
                        cbBusStop.IsEnabled = false;
                    }
                }
                else
                {
                    if (clockWorker.IsBusy)
                    {
                        clockWorker.CancelAsync();
                    }
                    timeEdit.Text = RunningTime.ToString();
                    shouldStop = true;
                    isStarted = false;
                    tbStart_Pause.Text = "Start";
                    timeDisplay.Visibility = Visibility.Collapsed;
                    timeEdit.Visibility = Visibility.Visible;
                    intervalSlider.IsEnabled = true;
                    cbBusStop.IsEnabled = true;
                }
            }
            catch (BO.ExceptionBL_UnexpectedProblem)
            {
                MessageBox.Show("Sorry, the program as met an unexpected problem. Please sign in again.");
            }
            catch // For unexpected issues
            {
                MessageBox.Show("An unexpected problem occured", "ERROR");
            }
        }

        int secondsInterval;
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



        private static bool shouldStop = true;

        // The background worker for the clock:
        private readonly BackgroundWorker clockWorker = new BackgroundWorker();

        /// <summary>
        /// The runClock background worker function
        /// </summary>
        private void runClock()
        {
            int t = 5;
            clockWorker.WorkerReportsProgress = true;
            clockWorker.WorkerSupportsCancellation = true;

            clockWorker.ProgressChanged += (sender, args) =>
            {
                secondsInterval = (int)intervalSlider.Value;
                timeDisplay.Text = RunningTime.ToString();
                RunningTime = RunningTime.Add(TimeSpan.FromSeconds(secondsInterval));
                if (RunningTime.Days > 0) RunningTime = RunningTime.Subtract(TimeSpan.FromDays(RunningTime.Days));
                if (t == 5) // To make the update according the interval (so for interval of 20 seconds, it will update every 5 seconds real-time, and for 1 - every 100 seconds real-time)
                {
                    changeTitleAsDayTime();
                    var minutesToBus = bl.GetLineTimingsPerStation(busStop, RunningTime);
                    if (minutesToBus.Count() == 0)
                        tbNoBuses.Visibility = Visibility.Visible;
                    else
                        tbNoBuses.Visibility = Visibility.Collapsed;
                    lvMinutesToBus.ItemsSource = minutesToBus;
                    t = 0;
                }
                t++;
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
                        clockWorker.ReportProgress(0);
                        try { Thread.Sleep(1000); } catch (Exception) { }
                    }
                }
            };

            if (clockWorker.IsBusy != true)
            {
                clockWorker.RunWorkerAsync();
            }


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
