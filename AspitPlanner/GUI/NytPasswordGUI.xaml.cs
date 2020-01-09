using AspitPlanner.Helpers;
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
    /// Interaction logic for NytPasswordGUI.xaml
    /// </summary>
    public partial class NytPasswordGUI : Window
    {
        public NytPasswordGUI(string Navn)
        {
            InitializeComponent();
            txtName.Text = Navn;
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdatePassword();
            }
        }

        private void CmdSkift(object sender, RoutedEventArgs e)
        {
            UpdatePassword();
        }

        private void UpdatePassword()
        {
            if(SQLDB.UpdateUserPassword(txtName.Text, txtPassword.Password))
            {
                this.Close();
            }
        }
    }
}
