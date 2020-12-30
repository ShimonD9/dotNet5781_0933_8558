﻿using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    partial class MainWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");
        public MainWindow()
        {
            InitializeComponent();
            lbBuses.DataContext = bl.GetAllBuses();
            lbBusStops.DataContext = bl.GetAllBusStops();
            lbBusLines.DataContext = bl.GetAllBusLines();
        }

        private void LBBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<BusDetailsWindow>().Any())
            {
                ListBox list = sender as ListBox;
                if (list != null)
                {
                    object item = list.SelectedItem; // Gets the selected item, and sends it to the new window builder
                    BusDetailsWindow busDetailsWindow = new BusDetailsWindow(item);
                    busDetailsWindow.ShowDialog();
                    lbBuses.ItemsSource = bl.GetAllBuses();
                }
            }
        }

        private void LBBusLines_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<BusLineDetailsWindow>().Any())
            {
                ListBox list = sender as ListBox;
                if (list != null)
                {
                    object item = list.SelectedItem; // Gets the selected item, and sends it to the new window builder
                    BusLineDetailsWindow busLineDetailsWindow = new BusLineDetailsWindow(item);
                    busLineDetailsWindow.ShowDialog();
                }
            }
        }

        private void LBBusStops_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<BusStopDetailsWindow>().Any())
            {
                ListBox list = sender as ListBox;
                if (list != null)
                {
                    object item = list.SelectedItem; // Gets the selected item, and sends it to the new window builder
                    BusStopDetailsWindow busStopDetailsWindow = new BusStopDetailsWindow(item);
                    busStopDetailsWindow.ShowDialog();
                }
            }
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
                lbBuses.ItemsSource = bl.GetAllBuses();
            }
        }

        /// <summary>
        /// The add bus line button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenAddBusLineWindow(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<AddBusLineWindow>().Any()) // To prevent the openning of another same window
            {
                AddBusLineWindow addBusLineWindow = new AddBusLineWindow(); // Creates the new window, and then shows it
                addBusLineWindow.ShowDialog();
                lbBuses.ItemsSource = bl.GetAllBuses();
                //lbBuses.Items.Refresh(); // For seeing the new bus added on the list view
            }
        }

        /// <summary>
        /// The add bus stop button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenAddBusStopWindow(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<AddBusStopWindow>().Any()) // To prevent the openning of another same window
            {
                AddBusStopWindow addBusStopWindow = new AddBusStopWindow(); // Creates the new window, and then shows it
                addBusStopWindow.ShowDialog();
                //lbBuses.Items.Refresh(); // For seeing the new bus added on the list view
            }
        }



    }
}
