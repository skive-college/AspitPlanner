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
    /// Interaction logic for Appointment.xaml
    /// </summary>
    public partial class AppointmentGUI : UserControl
    {
        
        public AppointmentGUI()
        {
            InitializeComponent();
            load();
        }

        public void load()
        {
            using (DBCon db = new DBCon())
            {
                CBStudent.DataContext = db.Students.OrderBy(s => s.Name).ToList();
            }
        }

        private void CmdCreate_Click(object sender, RoutedEventArgs e)
        {
            if(CBStudent.SelectedIndex != -1 && (Fri.IsChecked == true || VFU.IsChecked == true))
            {
                Appointment a = new Appointment();
                a.StudentID = (CBStudent.SelectedItem as Student).ID;
                a.FromeDate = (DateTime)dpFrom.SelectedDate;
                a.ToDate = (DateTime)dpTo.SelectedDate;

                TextRange textRange = new TextRange(txtInfo.Document.ContentStart, txtInfo.Document.ContentEnd);
                a.Info = textRange.Text;
                a.Day = getDays();
                a.Modules = getModules();
                

                using (DBCon db = new DBCon())
                {
                    string typenavn = "";
                    if (Fri.IsChecked == true)
                        typenavn = "fri";
                    else if (VFU.IsChecked == true)
                        typenavn = "vfu";
                    a.RegistrationTypeID = db.Types.Where(x => x.TypeName == typenavn).FirstOrDefault().ID;
                    db.Appointments.Add(a);
                    db.SaveChanges();
                    MainWindow.setStatus("aftale på " + (CBStudent.SelectedItem as Student).Name + " oprettet");
                }
                textRange.Text = "";
                clear();
            }
            else
            {
                MessageBox.Show("udfyld elev og en type Fri eller VFU");
            }
        }

        private string getModules()
        {
            string modul = "";
            if (Modul1.IsChecked == true)
            {
                modul += "M1,";
            }
            if (Modul2.IsChecked == true)
            {
                modul += "M2,";
            }
            if (Modul3.IsChecked == true)
            {
                modul += "M3,";
            }
            if (Modul4.IsChecked == true)
            {
                modul += "M4,";
            }
            if (modul.EndsWith(","))
            {
                modul = modul.Substring(0, modul.Length - 1);
            }
            return modul;
        }

        private string getDays()
        {
            string dage = "";
            if (Monday.IsChecked == true)
            {
                dage += "Monday,";
            }
            if (Tuesday.IsChecked == true)
            {
                dage += "Tuesday,";
            }
            if (Wednesday.IsChecked == true)
            {
                dage += "Wednesday,";
            }
            if (Thursday.IsChecked == true)
            {
                dage += "Thursday,";
            }
            if (Friday.IsChecked == true)
            {
                dage += "Friday";
            }
            if (dage.EndsWith(","))
            {
                dage = dage.Substring(0, dage.Length - 1);
            }
            return dage;
        }

        private void clear()
        {
            CBStudent.SelectedIndex = -1;
            dpFrom.SelectedDate = null;
            dpTo.SelectedDate = null;
            Monday.IsChecked = false;
            Tuesday.IsChecked = false;
            Wednesday.IsChecked = false;
            Thursday.IsChecked = false;
            Friday.IsChecked = false;
            Modul1.IsChecked = false;
            Modul2.IsChecked = false;
            Modul3.IsChecked = false;
            Modul4.IsChecked = false;
            Fri.IsChecked = false;
            VFU.IsChecked = false;
        }
        private void CMDShowAll_Click(object sender, RoutedEventArgs e)
        {
            AppointmentView av = new AppointmentView();
            av.ShowDialog();
        }
    }
}
