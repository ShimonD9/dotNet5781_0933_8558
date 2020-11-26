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
using System.Windows.Shapes;

namespace dotNet5781_03B_0933_8558
{
    /// <summary>
    /// Interaction logic for AddBusWindow.xaml
    /// </summary>
    public partial class AddBusWindow : Window
    {
        public AddBusWindow()
        {
            InitializeComponent();
            dateStart.DisplayDateEnd = DateTime.Now;
            dateLastTreat.DisplayDateEnd = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           //Bus newBus = new Bus(license.GetLineText(0), Double.Parse(mileageNow.GetLineText(0)), Double.Parse(mileageAtLastTreat.GetLineText(0)), dateStart.SelectedDate, dateLastTreat.SelectedDate);
           //MainWindow.busList.Add(newBus);
           this.Close();
        }

       private void dateStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dateChosen;
            if (dateStart.SelectedDate.HasValue)
            { 
                dateChosen = dateStart.SelectedDate.Value;
                if (dateChosen.Year < 2018)
                {
                    license.IsReadOnly = false;
                    license.Text = "";
                    license.MaxLength = 7;
                }
                else if(dateChosen.Year > 2017)
                {
                    license.IsReadOnly = false;
                    license.Text = "";
                    license.MaxLength = 8;
                }
            }
        }
    }
}
