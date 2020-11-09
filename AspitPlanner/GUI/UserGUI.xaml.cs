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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AspitPlanner.GUI
{
    /// <summary>
    /// Interaction logic for UserGUI.xaml
    /// </summary>
    public partial class UserGUI : UserControl
    {
        public UserGUI()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            cbRole.DataContext = SQLDB.GetUserRoles();
           
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if(txtName.Text != "" && txtPassword.Password != "")
            {
                User u = new User();
                u.Usernane = txtName.Text;
                u.Password = txtPassword.Password;

                if(cbRole.SelectedIndex != -1)
                {
                    UserRole ur = cbRole.SelectedValue as UserRole;
                    u.UserRole = ur.ID;
                }
                else
                {
                    // opret userRole og tag iDet
                    UserRole Ur = new UserRole();
                    Ur.Name = txtNewRole.Text;
                    u.UserRole = SQLDB.AddUserRole(Ur);                       
                    
                }
                SQLDB.AddUser(u);
                Clear();
                load();
                MainWindow.setStatus($"Bruger {u.Usernane} er oprettet");
                
        }
            


        }

        private void Clear()
        {
            txtName.Text = "";
            txtPassword.Password = "";
            txtNewRole.Text = "";
            cbRole.SelectedIndex = -1;
            
        }

        private void CbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
