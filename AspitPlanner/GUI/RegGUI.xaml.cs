﻿using AspitPlanner.Helpers;
using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
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
    /// Interaction logic for RegGUI.xaml
    /// </summary>
    public partial class RegGUI : UserControl
    {
        public RegGUI()
        {
            InitializeComponent();
            load();
        }

        public void load()
        {
             
            CBHold.DataContext = SQLDB.GetHold();
            CBModul1.DataContext = SQLDB.GetAbcentTypes();
            CBModul2.DataContext = SQLDB.GetAbcentTypes();
            CBModul3.DataContext = SQLDB.GetAbcentTypes();
            CBModul4.DataContext = SQLDB.GetAbcentTypes();

            Elever.DataContext = SQLDB.GetStudents();
                
            
        }

        private void CBHold_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            if(CBHold.SelectedIndex != -1)
            {
                clear();
                string Team = (CBHold.SelectedValue as Student).Team;

                Elever.DataContext = SQLDB.GetStundentsOnTeam(Team);                
            }
            else
            {               
                Elever.DataContext = SQLDB.GetStudents();               
            }
        }

        private void CmdClear_Click(object sender, RoutedEventArgs e)
        {
            clear();
            CBHold.SelectedIndex = -1;
        }

        private void CBModul_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != -1)
            {
                if (((sender as ComboBox).SelectedValue as AbsentType).TypeName == "Syg")
                {
                    if (CBModul2.SelectedIndex == -1)
                    {
                        CBModul2.SelectedIndex = (sender as ComboBox).SelectedIndex;
                    }
                    if (CBModul3.SelectedIndex == -1)
                    {
                        CBModul3.SelectedIndex = (sender as ComboBox).SelectedIndex;
                    }
                    if (CBModul4.SelectedIndex == -1)
                    {
                        CBModul4.SelectedIndex = (sender as ComboBox).SelectedIndex;
                    }
                }            
                updateStudent();
            }

        }

        private void updateStudent()
        {
            if (Elever.SelectedIndex != -1)
            {
                 
                DateTime today = getDateTime();

                Student student = (Elever.SelectedValue as Student);
                try
                {
                    Present p = SQLDB.getPressent(today, student.ID);
                    if (p != null)
                    {
                        if (CBModul1.SelectedIndex != -1)
                        {
                            p.Model1 = (CBModul1.SelectedValue as AbsentType).ID;
                        }
                        if (CBModul2.SelectedIndex != -1)
                        {
                            p.Model2 = (CBModul2.SelectedValue as AbsentType).ID;
                        }
                        if (CBModul3.SelectedIndex != -1)
                        {
                            p.Model3 = (CBModul3.SelectedValue as AbsentType).ID;
                        }
                        if (CBModul4.SelectedIndex != -1)
                        {
                            p.Model4 = (CBModul4.SelectedValue as AbsentType).ID;
                        }
                        try
                        {
                            if (txtNoteTilDag.Text != "")
                            {
                                ModulNote mn = new ModulNote();
                                mn.Date = p.Date;
                                mn.StudentID = p.StudentID;
                                mn.Note = txtNoteTilDag.Text;
                                SQLDB.AddOrUpdateModulNote(mn);

                            }
                            SQLDB.AddPresent(p);
                            MainWindow.setStatus($"{student.Name} {student.Team} er opdateret");
                        }
                        catch (Exception ex)
                        {
                            FileHandler.Error(ex);
                            FileHandler.Backup(p);
                            MainWindow.setStatus("Noget gik galt Backup taget og mail sent til sys admin");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileHandler.Error(ex);
                    MainWindow.setStatus("Noget gik galt");
                }
            }
        }
        

        private DateTime getDateTime()
        {
            DateTime d = DateTime.Now;

            d = new DateTime(d.Year, d.Month, d.Day);

            return d;
        }

        private void loadApointments(int ElevID)
        {
            DateTime today = getDateTime();
            var quary = from a in SQLDB.GetAppointments()
                        where a.StudentID.Equals(ElevID) && (a.FromeDate <= today) && a.ToDate >= today
                        select a;

            foreach(Appointment a in quary.ToList())
            {
                if(a.Day.Contains(today.DayOfWeek.ToString()))
                {
                    string[] moduler = a.Modules.Split(',');

                    int index = SQLDB.GetAftaleFri(a.RegistrationTypeID);
                    foreach(string s in moduler)
                    {
                        if(s == "M1")
                        {
                            CBModul1.SelectedIndex= index;

                        }
                        if (s == "M2")
                        {
                            CBModul2.SelectedIndex = index;

                        }
                        if (s == "M3")
                        {
                            CBModul3.SelectedIndex = index;

                        }
                        if (s == "M4")
                        {
                            CBModul4.SelectedIndex = index;

                        }
                    }
                }
            }
        }

        private void clear()
        {
            CBModul1.SelectedIndex = -1;
            CBModul2.SelectedIndex = -1;
            CBModul3.SelectedIndex = -1;
            CBModul4.SelectedIndex = -1;
            txtNoteTilDag.Text = "";
        }
        private void Elever_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Elever.SelectedIndex != -1)
            {
                clear();                
                int studentID = (Elever.SelectedValue as Student).ID;
                bool registered = false;
                 
                Present p = SQLDB.getPressent(getDateTime(), studentID);
                txtNoteTilDag.Text = SQLDB.GetModuleNotes(studentID, getDateTime()).Note;
                if (p != null)
                {
                    List<AbsentType> list = SQLDB.GetAbcentTypes();
                    for(int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ID == p.Model1)
                        {
                            registered = true;
                            CBModul1.SelectedIndex = i;
                        }
                        if (list[i].ID == p.Model2)
                        {
                            registered = true;
                            CBModul2.SelectedIndex = i;
                        }
                        if (list[i].ID == p.Model3)
                        {
                            registered = true;
                            CBModul3.SelectedIndex = i;
                        }
                                
                        if (list[i].ID == p.Model4)
                        {
                            registered = true;
                            CBModul4.SelectedIndex = i;
                        }
                                
                    }
                }
                else
                {
                    //creatNew();
                        
                }
                if(!registered)
                {
                    loadApointments(studentID);
                }
            }            
        }

       
        private void TxtNoteTilDag_LostFocus(object sender, RoutedEventArgs e)
        {
            updateStudent();
        }
    }
}
