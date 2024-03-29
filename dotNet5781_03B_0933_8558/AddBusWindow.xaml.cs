﻿using System;
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

            // Setting the dates in the date pickers so it will show the last date according the simulator running time:
            dateStart.DisplayDateEnd = MainWindow.useMyRunningDate;
            dateLastTreat.DisplayDateEnd = MainWindow.useMyRunningDate;
            dateStart.DisplayDate = MainWindow.useMyRunningDate;
            dateLastTreat.DisplayDate = MainWindow.useMyRunningDate;

        }

        /// <summary>
        /// The adding bus button method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDateChosen;
            DateTime treatDateChosen;
            if (!dateStart.SelectedDate.HasValue || !dateLastTreat.SelectedDate.HasValue) // Checks if the user chose a date
            {
                MessageBox.Show("You didn't fill the required date fields!", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                startDateChosen = dateStart.SelectedDate.Value;
                treatDateChosen = dateLastTreat.SelectedDate.Value;
                // Checks if the inputs are correct, and pops an appropriate message if not:
                if (startDateChosen.Year < 2018 && license.Text.Length < 7
                    || startDateChosen.Year > 2017 && license.Text.Length < 8)
                {
                    MessageBox.Show("The license you entered is too short!","Cannot add the bus",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
                else if (MainWindow.FindIfBusExist(MainWindow.busList, license.Text))
                {
                    MessageBox.Show("The bus license you entered already exists in the company!", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Double.TryParse(mileageNow.GetLineText(0), out double milNow) || !Double.TryParse(mileageAtLastTreat.GetLineText(0), out double milTreat))
                {
                    MessageBox.Show("You didn't fill correctly all the required information", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (double.Parse(mileageAtLastTreat.Text) > double.Parse(mileageNow.Text))
                {
                    MessageBox.Show("The total mileage cannot be smaller than the mileage at the last treat!", "Cannot add the bus", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    Bus newBus = new Bus(license.GetLineText(0), milNow, milTreat, startDateChosen, treatDateChosen)
                    {
                        KMLeftToTravel = fuel.Value * 12 // The info from the slider
                    };
                    MainWindow.busList.Insert(0, newBus);    // Inserts the new bus to the beginning of the list                 
                    this.Close();
                }
            }
        }

        // Using regex to unable wrongs inputs in the text box:

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/.]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberValidationTextBoxNoDots(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        
        /// <summary>
        /// The change of date event, helps with some style issues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dateChosen;
            if (dateStart.SelectedDate.HasValue)
            {
                license.IsEnabled = true;
                license.FontStyle = FontStyles.Normal;
                license.Text = "";
                dateChosen = dateStart.SelectedDate.Value;
                if (dateChosen.Year < 2018)
                {
                    license.MaxLength = 7;
                }
                else if (dateChosen.Year > 2017)
                {
                    license.MaxLength = 8;
                }
            }
        }
    }
}
