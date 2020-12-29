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

namespace PlGui
{
    /// <summary>
    /// Interaction logic for BusLineDetailsWindow.xaml
    /// </summary>
    public partial class BusLineDetailsWindow : Window
    {
        public BusLineDetailsWindow()
        {
            InitializeComponent();
        }

        // A second builder, to get the item selected in the list box
        public BusLineDetailsWindow(object item)
        {
            InitializeComponent();
            cbArea.ItemsSource = Enum.GetValues(typeof(BO.Enums.AREA));
            BusLineDet.DataContext = item;          
        }


        private void Button_Update(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {

        }
    }
}
