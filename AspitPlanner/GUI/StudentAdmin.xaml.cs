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
    /// Interaction logic for StudentAdmin.xaml
    /// </summary>
    public partial class StudentAdmin : UserControl
    {
        public StudentAdmin()
        {
            InitializeComponent();
        }

        private void cmbCreate_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text != "")
            {
                try
                {
                    using (DBCon db = new DBCon())
                    {
                        String Team = cbTeam.SelectedItem as String;
                        Student s = new Student() { Name = txtName.Text};

              
                        db.Students.Add(s);
                        db.SaveChanges();
                        txtName.Text = "";
                        cbTeam.SelectedItem = -1;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void CbTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}