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
    /// Interaction logic for PickUpBusWindow.xaml
    /// </summary>
    public partial class PickUpBusWindow : Window
    {
        public PickUpBusWindow()
        {
            InitializeComponent();
        }


        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var fxElt = sender as FrameworkElement;
            var fxElt2 = fxElt.Parent as FrameworkElement;
            Bus bus = fxElt2.DataContext as Bus;
            MessageBox.Show(bus.ToString());
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                double time = Math.Round(MainWindow.rnd.NextDouble() * 30 + 20, 2);
                //bus.Travel()
                this.Close();
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9/.]$");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
