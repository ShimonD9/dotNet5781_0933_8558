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

        IBL bl = BLFactory.GetBL("1"); // Calls and stores the instance of the bl interface

        /// <summary>
        /// Window ctor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
           
        }

        #region Sign in

        /// <summary>
        /// Eye image mouse enter event - will show the password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_MouseEnter(object sender, MouseEventArgs e)
        {
            tbPasswordShow.Text = pbPassword.Password;
            tbPasswordShow.Visibility = Visibility.Visible;
            pbPassword.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Eye image mouse enter event - won't show the password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_MouseLeave(object sender, MouseEventArgs e)
        {
            tbPasswordShow.Visibility = Visibility.Collapsed;
            pbPassword.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// In case the box is checked, no need to show the info label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbHuman_Checked(object sender, RoutedEventArgs e)
        {
            spInfo.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Sign in button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSignIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = tbUserName.GetLineText(0);
                string password = pbPassword.Password;

                // If the user filled the fields:
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password)) 
                {

                    // If the robot check box isn't checked:
                    if (cbHuman.IsChecked == false)
                    {
                        spInfo.Visibility = Visibility.Visible; // The info label made visible with the appropriate info
                        lblInfo.Content = "So... are you a zombie?";
                    }
                    else
                    {
                        // Asks for the user object from the bl
                        User user = bl.GetUser(userName);
                        
                        // If the passwords match:
                        if (user.Password == password) 
                        {
                            // If the user has manager access
                            if (user.ManageAccess == true && !Application.Current.Windows.OfType<AdminDisplayWindow>().Any()) // To prevent the openning of another same window
                            {
                                AdminDisplayWindow adminDisplayWindow = new AdminDisplayWindow(user); // Creates the new window, and then shows it
                                this.Close();
                                adminDisplayWindow.ShowDialog();
                            }
                            // If the user is a simple passenger
                            else if (user.ManageAccess == false && !Application.Current.Windows.OfType<PassengerUI_Window>().Any())
                            {
                                PassengerUI_Window passengerUI_Window = new PassengerUI_Window(user); // Creates the new window, and then shows it
                                this.Close();
                                passengerUI_Window.ShowDialog();
                            }
                        }
                        // If the passwords doesn't match
                        else
                        {
                            spInfo.Visibility = Visibility.Visible;
                            lblInfo.Content = "The password is incorrect. Please try again.";
                        }
                    }
                }
                // If the user didn't fill the fields as requiered
                else
                {
                    spInfo.Visibility = Visibility.Visible;
                    lblInfo.Content = "Please fill the required fields.";
                }
            }
            // If the user doesn't exist, an exception is being catched and the info printed
            catch (BO.ExceptionBL_UserKeyNotFound)
            {
                spInfo.Visibility = Visibility.Visible;
                lblInfo.Content = "The user name you have entered does not exist.";
            }
            catch
            {
                MessageBox.Show("An unexpected problem occured, please try again.", "Error");
            }
        }

        #endregion

        #region Sign up
        /// <summary>
        /// The sign up button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // If the sign in isn't enabled (it means the user is in a sign up proccess and now has to finish it)
                if (bSignIn.IsEnabled == false)
                {
                    // Absorbs the information from the text boxes:
                    string userName = tbUserName.GetLineText(0);
                    string passwordA = pbPassword.Password;
                    string passwordB = pbRepeatPassword.Password;

                    // If the use filled the fields:
                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passwordA) && !string.IsNullOrEmpty(passwordB))
                    {
                        // If the repeat passwords doens't match
                        if (passwordA != passwordB)
                        {
                            spInfo.Visibility = Visibility.Visible;
                            lblInfo.Content = "Passwords do not match.";
                        }
                        else
                        {
                            // If the check box isn't checked
                            if (cbHuman.IsChecked == false)
                            {
                                spInfo.Visibility = Visibility.Visible;
                                lblInfo.Content = "So... are you a zombie?";
                            }
                            // Starts the sign up proccess - creates a BO.User, initializing it and sends it to bl.AddUser
                            else
                            {
                                BO.User user = new BO.User();
                                user.UserName = userName;
                                user.Password = passwordA;
                                bl.AddUser(user);

                                // Changes back to the original interface
                                spInfo.Visibility = Visibility.Visible;
                                lblInfo.Content = "Success!";
                                lblInfo.Foreground = Brushes.Green;
                                spRepeatPassword.Visibility = Visibility.Collapsed;
                                bSignIn.IsEnabled = true;
                            }
                        }
                    }
                    // The user didn't fill the fields:
                    else
                    {
                        spInfo.Visibility = Visibility.Visible;
                        lblInfo.Content = "Please fill the required fields.";
                    }
                }
                // If the user only began the sign up proccess, it makes the reapet password panel visible and disables the sign in button
                else
                {
                    spRepeatPassword.Visibility = Visibility.Visible;
                    bSignIn.IsEnabled = false;
                }
            }
            // If the user already exist
            catch (BO.ExceptionBL_UserAlreadyExist)
            {
                spInfo.Visibility = Visibility.Visible;
                lblInfo.Content = "The user name is already in use";
            }
        }

        /// <summary>
        /// Cancel sign up button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            spRepeatPassword.Visibility = Visibility.Collapsed; // Collapsing the reapet password panel
            bSignIn.IsEnabled = true; // The sign in button is enabled again
        }

        #endregion

        /// <summary>
        /// The info panel mouse leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spInfo_MouseLeave(object sender, MouseEventArgs e)
        {
            spInfo.Visibility = Visibility.Collapsed;
            lblInfo.Foreground = Brushes.DarkBlue;
        }

    }
}
