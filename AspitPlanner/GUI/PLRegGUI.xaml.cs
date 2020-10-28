using AspitPlanner.Helpers;
using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
using System.Windows.Threading;

namespace AspitPlanner.GUI
{
    /// <summary>
    /// Interaction logic for PLRegGUI.xaml
    /// </summary>
    public partial class PLRegGUI : UserControl
    {
        private bool done;
        public PLRegGUI()
        {
            InitializeComponent();
        }
        
        public void load()
        {
            using (DBCon db = new DBCon())
            {
                dpDag.SelectedDate = DateTime.Now;
                CBHold.DataContext = db.GetHold();
                CBModul1.DataContext = db.GetAbcentTypes();
                CBModul2.DataContext = db.GetAbcentTypes();
                CBModul3.DataContext = db.GetAbcentTypes();
                CBModul4.DataContext = db.GetAbcentTypes();
                
                Elever.DataContext = db.getNotPressent(getDateTime());
                //load apointmens på alle elever

            }
        }

        private void CBHold_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBHold.SelectedIndex != -1)
            {
                clear();
                string Team = (CBHold.SelectedValue as Student).Team;

                using (DBCon db = new DBCon())
                {
                    Elever.DataContext = db.GetStundentsOnTeam(Team);
                }
            }
            else
            {
                using (DBCon db = new DBCon())
                {
                    Elever.DataContext = db.getNotPressent(getDateTime());
                }
            }
        }

        private void CmdClear_Click(object sender, RoutedEventArgs e)
        {
            clear();
            dpDag.SelectedDate = DateTime.Now;
            CBHold.SelectedIndex = -1;
        }

        private void CBModul_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Elever.SelectedIndex != -1 && (sender as ComboBox).SelectedIndex != -1)
            {
                using (DBCon db = new DBCon())
                {
                    DateTime today = getDateTime();
                    int studentID = (Elever.SelectedValue as Student).ID;
                    Present p = db.getPressent(today, studentID);

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
                    else if (((sender as ComboBox).SelectedValue as AbsentType).TypeName == "Udeblevet")
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
        }

        
       

        private DateTime getDateTime()
        {
            DateTime d = (DateTime)dpDag.SelectedDate;

            d = new DateTime(d.Year, d.Month, d.Day);

            return d;
        }

        private void loadApointments(int ElevID)
        {
            using (DBCon db = new DBCon())
            {
                DateTime today = getDateTime();
                var quary = from a in db.Appointments
                            where a.StudentID.Equals(ElevID) && (a.FromeDate <= today) && a.ToDate >= today
                            select a;

                foreach (Appointment a in quary.ToList())
                {
                    if (a.Day.Contains(today.DayOfWeek.ToString()))
                    {
                        string[] moduler = a.Modules.Split(',');

                        int index = db.GetAftaleFri(a.RegistrationTypeID);
                        foreach (string s in moduler)
                        {
                            if (s == "M1")
                            {
                                CBModul1.SelectedIndex = index;

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
            if (Elever.SelectedIndex != -1)
            {
                clear();
                int studentID = (Elever.SelectedValue as Student).ID;
                bool registered = false;
                using (DBCon db = new DBCon())
                {
                    Present p = db.getPressent(getDateTime(), studentID);
                    if (p != null)
                    {

                        for (int i = 0; i < db.GetAbcentTypes().Count; i++)
                        {
                            if (db.GetAbcentTypes()[i].ID == p.Model1)
                            {
                                registered = true;
                                CBModul1.SelectedIndex = i;
                            }
                            if (db.GetAbcentTypes()[i].ID == p.Model2)
                            {
                                registered = true;
                                CBModul2.SelectedIndex = i;
                            }
                            if (db.GetAbcentTypes()[i].ID == p.Model3)
                            {
                                registered = true;
                                CBModul3.SelectedIndex = i;
                            }

                            if (db.GetAbcentTypes()[i].ID == p.Model4)
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
                    if (!registered)
                    {
                        loadApointments(studentID);
                    }
                }
            }

        }

        private void DpDag_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            using (DBCon db = new DBCon())
            {
                clear();
                Elever.DataContext = db.getNotPressent(getDateTime());
            }
        }

        private void CmdOpdater_Click(object sender, RoutedEventArgs e)
        {
            load();
        }
        private string getNote(DateTime time, int sID)
        {
            string retur = "";
            using (DBCon db = new DBCon())
            {

                ModulNote mn = db.ModulNotes.Where(x => x.StudentID == sID && x.Date == time).FirstOrDefault();
                if (mn != null)
                    retur = mn.Note;
            }
            return retur;
        }

        private void updateStudent()
        {
            if (Elever.SelectedIndex != -1)
            {
                using (DBCon db = new DBCon())
                {
                    DateTime today = getDateTime();

                    Student student = (Elever.SelectedValue as Student);
                    try
                    {
                        Present p = db.getPressent(today, student.ID);
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
                                    db.ModulNotes.AddOrUpdate(mn);
                                    db.SaveChanges();

                                }
                                db.Presents.AddOrUpdate(p);
                                db.SaveChanges();
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
        }
        private void TxtNoteTilDag_LostFocus(object sender, RoutedEventArgs e)
        {
            updateStudent();
        }
    }
}
