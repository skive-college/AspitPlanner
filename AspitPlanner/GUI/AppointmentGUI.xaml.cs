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
                CBStudent.DataContext = db.Students.ToList();
            }
        }

        private void CmdCreate_Click(object sender, RoutedEventArgs e)
        {
            if(CBStudent.SelectedIndex != -1)
            {
                Appointment a = new Appointment();
                a.StudentID = (CBStudent.SelectedItem as Student).ID;
                a.FromeDate = (DateTime)dpFrom.SelectedDate;
                a.ToDate = (DateTime)dpTo.SelectedDate;
                //Insert day here
                //Insert modules here
                TextRange textRange = new TextRange(txtInfo.Document.ContentStart, txtInfo.Document.ContentEnd);
                a.Info = textRange.Text;

                using (DBCon db = new DBCon())
                {
                    db.Appointments.Add(a);
                    db.SaveChanges();
                }
                CBStudent.SelectedIndex = -1;
                dpFrom.SelectedDate = null;
                dpTo.SelectedDate = null;
                //Insert day here
                //Insert modules here
                textRange.Text = "";
            }
        }
    }
}
