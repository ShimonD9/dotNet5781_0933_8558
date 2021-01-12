using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        DateTime inputTime;
        BO.User passenger;
        

        public PassengerUI_Window()
        {
            InitializeComponent();
        }

        public PassengerUI_Window(BO.User user)
        {
            InitializeComponent();
            passenger = user;
            changeTitleAsDayTime();
            timeEdit.Text = DateTime.Now.ToString("hh:mm:ss");
            cbBusStop.ItemsSource = bl.GetAllBusStops().OrderBy(busStop => busStop.BusStopKey);
        }



      
        private void Start_Pause_Click(object sender, RoutedEventArgs e)
        {
            if (tbStart_Pause.Text == "Start")
            {
                if (!DateTime.TryParse(timeEdit.Text, out inputTime))
                    MessageBox.Show("Wront time input!");
                else
                {
                    tbStart_Pause.Text = "Pause";
                    timeDisplay.Visibility = Visibility.Visible;
                    timeEdit.Visibility = Visibility.Collapsed;

                    int interval = (int)intervalSlider.Value;
                }
            }
            else if (tbStart_Pause.Text == "Pause")
            {
                tbStart_Pause.Text = "Start";
                timeDisplay.Visibility = Visibility.Collapsed;
                timeEdit.Visibility = Visibility.Visible;
            }
        }

        private void changeTitleAsDayTime()
        {

            string time = "Hello,";
            if (DateTime.Now.Hour > 5 && DateTime.Now.Hour < 12)
                time = "Good morning, ";
            else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 17)
                time = "Good afternoon, ";
            else if (DateTime.Now.Hour >= 17 && DateTime.Now.Hour < 20)
                time = "Good evening, ";
            else if (DateTime.Now.Hour >= 20 && DateTime.Now.Hour < 5)
                time = "Good night, ";
            PassengerWindow.Title = time + passenger.UserName;
        }

        private void NumberValidationTextBoxColon(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/:]$");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
