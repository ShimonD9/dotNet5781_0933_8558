﻿using System;
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
    /// Interaction logic for BusDetailsWindow.xaml
    /// </summary>
    public partial class BusDetailsWindow : Window
    {
        public BusDetailsWindow()
        {
            InitializeComponent();
        }

        public BusDetailsWindow(object item)
        {
            InitializeComponent();
            BusDet.DataContext = item;
        }
    }
}
