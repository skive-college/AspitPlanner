using AspitPlanner.Helpers;
using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            load();
            loadTeams();
        }

        private void LoadInactiveStudents()
        {
            IList<Student> students = SQLDB.GetInactiveStudents();
            inactiveStudentsListView.ItemsSource = null;
            inactiveStudentsListView.ItemsSource = students;
        }

        private void cmbCreate_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text != "")
            {
                try
                {
                      
                    String Team;
                    if (txtNewTeam.Text != "")
                    {
                        Team = txtNewTeam.Text;
                    }
                    else
                    {
                        Team = (cbTeam.SelectedValue as Student).Team;
                    }
                    Student s = new Student() { Name = txtName.Text, Team = Team, Aktiv = true};

                    SQLDB.addStudent(s);
                    clear();
                    load();                        
                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void load()
        {
            lwStudent.DataContext = SQLDB.GetStudents();
            LoadInactiveStudents();
            loadTeams();
        }

        private void clear()
        {

            txtName.Text = "";
            txtNewTeam.Text = "";
            cbTeam.SelectedIndex = -1;
        }
        public void loadTeams()
        {
            List<Student> liste = SQLDB.GetStudents();
            var Teams = liste.GroupBy(test => test.Team)
                    .Select(grp => grp.First())
                    .ToList();
            cbTeam.DataContext = Teams;
            
        }

        private void LwStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SletButton.IsEnabled = inactiveStudentsListView.SelectedItem != null;
            cmdInactiv.IsEnabled = lwStudent.SelectedItem != null;
        }

        private void CbTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmdInactiv_Click(object sender, RoutedEventArgs e)
        {
            if(lwStudent.SelectedIndex != -1)
            {
                Student s = lwStudent.SelectedItem as Student;
                SQLDB.SetInactiv(s);
                clear();
                load();
            }
        }

        private void inactiveStudentsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SletButton.IsEnabled = inactiveStudentsListView.SelectedItem != null;
            ActivateStudentButton.IsEnabled = inactiveStudentsListView.SelectedItem != null;
        }

        private void SletButton_Click(object sender, RoutedEventArgs e)
        {
            if (inactiveStudentsListView.SelectedItem is Student s)
            {
                SQLDB.DeleteStudentData(s.ID);
                load();
            }
        }

        private void ActivateStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (inactiveStudentsListView.SelectedItem is Student s)
            {
                SQLDB.ReActivateStudent(s.ID);
                load();
            }
        }
    }
}