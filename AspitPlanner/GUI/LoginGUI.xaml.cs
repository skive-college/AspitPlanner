using AspitPlanner.Helpers;
using AspitPlanner.Models;
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

namespace AspitPlanner.GUI
{
    /// <summary>
    /// Interaction logic for LoginGUI.xaml
    /// </summary>
    public partial class LoginGUI : Window
    {
        User u = null;
        
        public LoginGUI()
        {
            InitializeComponent();
        }

        private void validate()
        {
            User us = new User();
            if (txtName.Text != "" && txtPassword.Password != "")
            {
                us.Usernane = txtName.Text;

                us.Password = txtPassword.Password;
            }
            try
            {
                u = SQLDB.GetUser(us);
                if(u != null && u.Password == "1234")
                {
                    DialogPassword dialog = new DialogPassword();
                    if (dialog.ShowDialog() == true)
                    {
                            
                        u.Password = dialog.Password;
                        SQLDB.UpdateUserPassword(u.Usernane,u.Password);
                    }
                }
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }

            if(u!= null)
            {
                DialogResult = true;
            }
            else
            {
                txtPassword.Password = "";
                txtWrong.Text = "Forkert login";
                txtName.SelectAll();
                txtName.Focus();

            }
            
        }

        public User GetUser()
        {
            return u;
        }

        private void CmdLogin(object sender, RoutedEventArgs e)
        {
            validate();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(DialogResult != true)
            {
                DialogResult = false;
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                validate();
            }
        }
    }
}
