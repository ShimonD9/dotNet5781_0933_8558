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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           Bus newBus = new Bus(license.GetLineText(0), Double.Parse(mileageNow.GetLineText(0)), Double.Parse(mileageAtLastTreat.GetLineText(0)), dateStart.DisplayDate, dateLastTreat.DisplayDate);
           MainWindow.busList.Add(newBus);
           this.Close();
        }
    }
}
