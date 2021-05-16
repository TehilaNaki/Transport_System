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
using BLAPI;

namespace PLGUI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        IBL bl = BLFactory.GetBL();
        BO.User MyUser;
        public Login()
        {
            InitializeComponent();

        }
        /// <summary>
        /// an event to enter to the main window
        /// </summary>

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            //if the user did not fill in the name or password 
            if (password.Password.Length == 0 || username.Text == "")
                MessageBox.Show("Please enter user name and password");
            else
            {
                try
                {
                    MyUser = bl.GetUser(username.Text);
                    if (MyUser.UserName == username.Text && MyUser.password == password.Password)
                    {
                        if (MyUser.Admin == BO.Permission.Managment)
                        {
                            MainWindow main = new MainWindow(MyUser);
                            main.Show();
                        }
                        else
                        {
                            Passenger passenger = new Passenger(MyUser);
                            passenger.Show();
                        }
                        Close();
                    }
                    else MessageBox.Show("Incorrect username or password");
                }
                catch (BO.BadUserNameException ex)
                {
                    MessageBox.Show(ex.Message);
                    username.Text = "";
                    password.Password = "";
                }

            }
        }
        /// <summary>
        /// an event that save the user and enter 
        /// </summary>

        private void SaveEnter_Click(object sender, RoutedEventArgs e)
        {

            if (passwordBox.Password.Length == 0 || username1.Text == "")
                MessageBox.Show("Please enter user name and password");
            else
            {
                if (MyUser == null)
                    MyUser = new BO.User();
                try
                {
                    MyUser.password = passwordBox.Password;
                    MyUser.UserName = username1.Text;
                    if ((checkM.IsChecked == true) && (managerP.Password == "buses"))
                    {
                        MyUser.Admin = BO.Permission.Managment;

                        bl.AddUser(MyUser);
                        MessageBox.Show("Manager user:" + MyUser.UserName + " added succsesfully!");
                        MainWindow main = new MainWindow(MyUser);
                        main.Show();
                        checkM.IsChecked = false;
                        passwordBox.Password = "";
                        username1.Text = "";
                        Close();
                    }
                    else if (checkM.IsChecked == true) MessageBox.Show("Incorrect manager pin");
                    else
                    {
                        MyUser.Admin = BO.Permission.Passenger;
                        MyUser.password = passwordBox.Password;
                        bl.AddUser(MyUser);
                        MessageBox.Show("Passenger user:" + MyUser.UserName + " added succsesfully!");
                        checkM.IsChecked = false;
                        passwordBox.Password = "";
                        Passenger passenger = new Passenger(MyUser);
                        passenger.Show();
                        Close();
                    }
                }
                catch (BO.BadUserNameException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }

}