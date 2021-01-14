using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
           
        }



        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            tbPasswordShow.Text = pbPassword.Password;
            tbPasswordShow.Visibility = Visibility.Visible;
            pbPassword.Visibility = Visibility.Collapsed;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            tbPasswordShow.Visibility = Visibility.Collapsed;
            pbPassword.Visibility = Visibility.Visible;
        }



        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = tbUserName.GetLineText(0);
                string password = pbPassword.Password;
                if (userName != "" && password != "")
                {
                    if (cbHuman.IsChecked == false)
                    {
                        spInfo.Visibility = Visibility.Visible;
                        lblInfo.Content = "So... are you a zombie?";
                    }
                    else
                    {
                        BO.User user = bl.GetUser(userName);
                        if (user.Password == password)
                        {
                            if (user.ManageAccess == true && !Application.Current.Windows.OfType<AdminDisplayWindow>().Any()) // To prevent the openning of another same window
                            {
                                AdminDisplayWindow adminDisplayWindow = new AdminDisplayWindow(user); // Creates the new window, and then shows it
                                this.Close();
                                adminDisplayWindow.ShowDialog();
                            }
                            else if (!Application.Current.Windows.OfType<PassengerUI_Window>().Any())
                            {
                                PassengerUI_Window passengerUI_Window = new PassengerUI_Window(user); // Creates the new window, and then shows it
                                this.Close();
                                passengerUI_Window.ShowDialog();
                            }
                        }
                        else
                        {
                            spInfo.Visibility = Visibility.Visible;
                            lblInfo.Content = "The password is incorrect. Please try again.";
                        }
                    }
                }
                else
                {
                    spInfo.Visibility = Visibility.Visible;
                    lblInfo.Content = "Please fill the required fields.";
                }
            }
            catch (BO.ExceptionBL_UserKeyNotFound)
            {
                spInfo.Visibility = Visibility.Visible;
                lblInfo.Content = "The user name you have entered does not exist.";
            }
        }

        private void cbHuman_Checked(object sender, RoutedEventArgs e)
        {
            spInfo.Visibility = Visibility.Collapsed;
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bSignIn.IsEnabled == false)
                {
                    string userName = tbUserName.GetLineText(0);
                    string passwordA = pbPassword.Password;
                    string passwordB = pbRepeatPassword.Password;
                    if (userName != "" && passwordA != "" && passwordB != "")
                    {
                        if (passwordA != passwordB)
                        {
                            spInfo.Visibility = Visibility.Visible;
                            lblInfo.Content = "Passwords do not match.";
                        }
                        else
                        {
                            if (cbHuman.IsChecked == false)
                            {
                                spInfo.Visibility = Visibility.Visible;
                                lblInfo.Content = "So... are you a zombie?";
                            }
                            else
                            {
                                BO.User user = new BO.User();
                                user.UserName = userName;
                                user.Password = passwordA;
                                bl.AddUser(user);
                                spInfo.Visibility = Visibility.Visible;
                                lblInfo.Content = "Success!";
                                lblInfo.Foreground = Brushes.Green;
                                spRepeatPassword.Visibility = Visibility.Collapsed;
                                bSignIn.IsEnabled = true;
                            }
                        }
                    }
                    else
                    {
                        spInfo.Visibility = Visibility.Visible;
                        lblInfo.Content = "Please fill the required fields.";
                    }
                }
                else
                {
                    spRepeatPassword.Visibility = Visibility.Visible;
                    bSignIn.IsEnabled = false;
                }
            }
            catch (BO.ExceptionBL_UserAlreadyExist)
            {
                spInfo.Visibility = Visibility.Visible;
                lblInfo.Content = "The user name is already in use";
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            spRepeatPassword.Visibility = Visibility.Collapsed;
            bSignIn.IsEnabled = true;
        }

        private void spInfo_MouseLeave(object sender, MouseEventArgs e)
        {
            spInfo.Visibility = Visibility.Collapsed;
            lblInfo.Foreground = Brushes.DarkBlue;
        }

    }
}
