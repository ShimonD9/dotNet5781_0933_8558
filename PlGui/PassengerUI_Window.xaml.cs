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
    /// Interaction logic for PassengerUI_Window.xaml
    /// </summary>
    public partial class PassengerUI_Window : Window
    {
        IBL bl = BLFactory.GetBL("1");
        BO.User passenger;

        public PassengerUI_Window()
        {
            InitializeComponent();
        }

        public PassengerUI_Window(BO.User user)
        {
            InitializeComponent();

            passenger = user;
            string time = "Hello,";
            if (DateTime.Now.Hour > 5 && DateTime.Now.Hour < 12)
                time = "Good morning, ";
            else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 17)
                time = "Good afternoon, ";
            else if (DateTime.Now.Hour >= 17 && DateTime.Now.Hour < 20)
                time = "Good evening, ";
            else if (DateTime.Now.Hour >= 20 && DateTime.Now.Hour < 5)
                time = "Good night, ";
            PassengerWindow.Title = time + user.UserName;

            cbBusStop.ItemsSource = bl.GetAllBusStops().OrderBy(busStop => busStop.BusStopKey);

            System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100); // 100 Milliseconds  
            myDispatcherTimer.Tick += myDispatcherTimer_Tick;
            myDispatcherTimer.Start();
        }

        void myDispatcherTimer_Tick(object sender, EventArgs e)
        {
            tbk_clock.Text = DateTime.Now.ToString("hh:mm:ss");
        }
    }
}
