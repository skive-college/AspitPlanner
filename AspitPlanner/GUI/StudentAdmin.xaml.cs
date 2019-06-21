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

        private void cmbCreate_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text != "")
            {
                try
                {
                    using (DBCon db = new DBCon())
                    {
                        String team;

                        if(cbTeam.SelectedIndex == -1)
                        {
                            team = "A";
                            if (DateTime.Now.Month < 7)
                                team += "F";
                            else
                                team += "E";

                            team += (DateTime.Now.Year % 100);

                        }
                        else
                        {
                            team = (cbTeam.SelectedValue as Student).Team;
                        }
                        Student s = new Student() { Name = txtName.Text, Team = team};

              
                        db.Students.Add(s);
                        db.SaveChanges();
                        clear();
                        load();                        
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void load()
        {
            using (DBCon db = new DBCon())
            {
                lwStudent.DataContext = db.Students.ToList();
            }
            loadTeams();
        }

        private void clear()
        {

            txtName.Text = "";
            cbTeam.SelectedIndex = -1;
        }
        public void loadTeams()
        {
            using (DBCon db = new DBCon())
            {
                List<Student> liste = db.Students.ToList();
                var Teams = liste.GroupBy(test => test.Team)
                       .Select(grp => grp.First())
                       .ToList();
                cbTeam.DataContext = Teams;
            }
        }

        private void LwStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void CbTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}