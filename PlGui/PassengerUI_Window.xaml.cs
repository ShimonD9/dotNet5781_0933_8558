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
        IBL bl = BLFactory.GetBL("1");
        BO.User passenger;
        BO.BusStop busStop;



        public PassengerUI_Window()
        {
            InitializeComponent();
        }

        public PassengerUI_Window(BO.User user)
        {
            InitializeComponent();
            passenger = user;
            changeTitleAsDayTime();
            timeEdit.Text = DateTime.Now.ToString("HH:mm:ss");
            cbBusStop.ItemsSource = bl.GetAllBusStops().OrderBy(busStop => busStop.BusStopKey);
        }




        private void Start_Pause_Click(object sender, RoutedEventArgs e)
        {
            if (tbStart_Pause.Text == "Start")
            {
                if (!TimeSpan.TryParse(timeEdit.Text, out TimeSpan inputTime))
                    MessageBox.Show("Wrong time input!");
                else
                {
                    RunningTime = inputTime;
                    shouldStop = false;
                    secondsInterval = (int)intervalSlider.Value;
                    RunClock();
                    tbStart_Pause.Text = "Pause";
                    timeDisplay.Visibility = Visibility.Visible;
                    timeEdit.Visibility = Visibility.Collapsed;
                    intervalSlider.IsEnabled = false;
                    cbBusStop.IsEnabled = false;
                }
            }
            else if (tbStart_Pause.Text == "Pause")
            {
                timeEdit.Text = RunningTime.ToString();
                shouldStop = true;
                tbStart_Pause.Text = "Start";
                timeDisplay.Visibility = Visibility.Collapsed;
                timeEdit.Visibility = Visibility.Visible;
                intervalSlider.IsEnabled = true;
                cbBusStop.IsEnabled = true;
            }
        }

        private void changeTitleAsDayTime()
        {

            string time = "Hello, ";
            if (DateTime.Now.Hour > 5 && DateTime.Now.Hour < 12)
                time = "Good morning, ";
            else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 17)
                time = "Good afternoon, ";
            else if (DateTime.Now.Hour >= 17 && DateTime.Now.Hour < 20)
                time = "Good evening, ";
            else if (DateTime.Now.Hour >= 20 || DateTime.Now.Hour < 5)
                time = "Good night, ";
            PassengerWindow.Title = time + passenger.UserName;
        }

        private void NumberValidationTextBoxColon(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/:]$");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void cbBusStop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbNoBuses.Visibility = Visibility.Collapsed;
            busStop = cbBusStop.SelectedItem as BO.BusStop;
            lvLinesStopHere.DataContext = busStop;
            var minutesToBus = bl.GetLineTimingsPerStation(busStop, runningTime);
            if (minutesToBus.Count() == 0)
                tbNoBuses.Visibility = Visibility.Visible;
            lvMinutesToBus.ItemsSource = minutesToBus;
            //shouldStop = true;
        }

        //////////////////////////////////////////////// CLOCK ////////////////////////////////////////////////
        ///

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



        public static bool shouldStop = true;

        // The background worker for the clock:
        private readonly BackgroundWorker clockWorker = new BackgroundWorker();

        public void RunClock()
        {
            int t = 60 / secondsInterval;
            clockWorker.WorkerReportsProgress = true;

            clockWorker.ProgressChanged += (sender, args) =>
            {
                timeDisplay.Text = RunningTime.ToString();
                RunningTime = RunningTime.Add(TimeSpan.FromSeconds(secondsInterval));
                if (t == 60 / secondsInterval)
                {
                    var minutesToBus = bl.GetLineTimingsPerStation(busStop, RunningTime);
                    if (minutesToBus.Count() == 0)
                        tbNoBuses.Visibility = Visibility.Visible;
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
