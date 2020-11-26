using System;
using System.Collections.Generic;
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
            DateTime startDateChosen;
            DateTime treatDateChosen;
            if (!dateStart.SelectedDate.HasValue || !dateLastTreat.SelectedDate.HasValue)
            {
                MessageBox.Show("You didn't select a date!");
            }
            else
            {
                startDateChosen = dateStart.SelectedDate.Value;
                treatDateChosen = dateLastTreat.SelectedDate.Value;
                if (startDateChosen.Year < 2018 && license.Text.Length < 7
                    || startDateChosen.Year > 2017 && license.Text.Length < 8)
                {
                    MessageBox.Show("The license you entered is too short!");
                }
                else if (mileageNow.Text == "" || mileageAtLastTreat.Text == "")
                {
                    MessageBox.Show("You didn't fill all the required information");
                }
                else if (double.Parse(mileageAtLastTreat.Text) > double.Parse(mileageNow.Text))
                {
                    MessageBox.Show("The total mileage cannot be smaller than the mileage at the last treat!");
                }
                else
                {
                    Bus newBus = new Bus(license.GetLineText(0), double.Parse(mileageNow.GetLineText(0)), double.Parse(mileageAtLastTreat.GetLineText(0)), startDateChosen, treatDateChosen);
                    MainWindow.busList.Add(newBus);
                    this.Close();
                }
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            ..Regex regex = new Regex("[^0-9]+ [\.?]");
            e.Handled = regex.IsMatch(e.Text);
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
                    license.FontStyle = FontStyles.Normal;
                    license.Text = "";
                    license.MaxLength = 7;
                }
                else if(dateChosen.Year > 2017)
                {
                    license.IsReadOnly = false;
                    license.FontStyle = FontStyles.Normal;
                    license.Text = "";
                    license.MaxLength = 8;
                }
            }
        }
    }
}
