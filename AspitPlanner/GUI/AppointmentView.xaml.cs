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
    /// Interaction logic for AppointmentView.xaml
    /// </summary>
    public partial class AppointmentView : Window
    {
        public AppointmentView()
        {
            InitializeComponent();
            RegistrationsGrid.AutoGeneratingColumn += RegistrationsGrid_AutoGeneratingColumn; 
            load();
            loadStudents();
        }

        private void RegistrationsGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "ID" || e.Column.Header.ToString() == "StudentID")
                e.Cancel = true;
        }

        private void load()
        {
            if (CBStudentApp.SelectedIndex != -1)
            {
                
                Student s = CBStudentApp.SelectedValue as Student;
                RegistrationsGrid.DataContext = SQLDB.getAllPresents(s.ID);
                
            }
            else
            {
                
                RegistrationsGrid.DataContext = SQLDB.getAllPresents(-1);
                
            }
        }

        private void loadStudents()
        {
            CBStudentApp.DataContext = SQLDB.GetStudents();            
        }
        private void CBStudentApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CBStudentApp.SelectedIndex != -1)
            {

                ; 
                Student s = CBStudentApp.SelectedValue as Student;
                RegistrationsGrid.DataContext = SQLDB.getAllPresents(s.ID);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            CBStudentApp.SelectedIndex = -1;
            load();
        }

        private void CmdSlet_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("er du sikker på du vil slette denne aftale", "info", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if(result == MessageBoxResult.OK)
            {
                if (RegistrationsGrid.SelectedIndex != -1)
                {
                    AppointmentStudent apstud = RegistrationsGrid.SelectedValue as AppointmentStudent;

                    SQLDB.deleteAppointment(apstud.ID);
                    load();
                }
            }
            
        }

        private void cmdStop_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("er du sikker på du vil Stoppe denne aftale", "info", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                if (RegistrationsGrid.SelectedIndex != -1)
                {
                    AppointmentStudent apstud = RegistrationsGrid.SelectedValue as AppointmentStudent;
                    
                    SQLDB.updateAppointment(apstud, dpStopDato.SelectedDate);
                    dpStopDato.SelectedDate = null;
                    load();
                }
            }
        }

        
    }
}
