using AspitPlanner.Helpers;
using AspitPlanner.Models;
using Microsoft.Reporting.WinForms;
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
    /// Interaction logic for statistic.xaml
    /// </summary>
    public partial class statistic : UserControl
    {
        List<CheckBox> checkboxes = new List<CheckBox>();
        public statistic()
        {
            InitializeComponent();
            loadStudents();
        }

        public void Load()
        {
            loadStudents();
            tilDato.Text = "";
            fraDato.Text = "";
            tilDato.SelectedDate = null;
            fraDato.SelectedDate = null;


        }
        private void loadStudents()
        {
            using (DBCon db = new DBCon())
            {
                cbSElev.DataContext = db.Students.ToList();
                seek();
            }
        }

      

        private void C_Checked(object sender, RoutedEventArgs e)
        {
            seek();
        }

        private void seek()
        {
            using (DBCon db = new DBCon())
            {
                try
                {
                    
                    _reportViewer.Clear();
                    _reportViewer.LocalReport.DataSources.Clear();
                    List<StudentStatistic> stats = new List<StudentStatistic>();
                    if (cbSElev.SelectedIndex != -1)
                    {
                        Student s = cbSElev.SelectedValue as Student;
                        stats = db.getStatistics(s, fraDato.SelectedDate, tilDato.SelectedDate);
                    }
                    else
                    {
                        stats = db.getStatistics(null, fraDato.SelectedDate, tilDato.SelectedDate);
                    }

                    ReportDataSource reportDataSource = new ReportDataSource("DataSet", stats);
                    
                    _reportViewer.LocalReport.DataSources.Add(reportDataSource);

                    _reportViewer.LocalReport.ReportPath = "../../report.rdlc";

                    _reportViewer.RefreshReport();
                }
                catch (Exception ex)
                {
                    FileHandler.Error(ex);
                }
            }
            
        }

        private void CbSElev_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            seek();            
        }

        private void FraDato_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            seek();
        }

        private void TilDato_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            seek();
        }

        private void CmdSeek_Click(object sender, RoutedEventArgs e)
        {
            using (DBCon db = new DBCon())
            {
                if (cbSElev.SelectedIndex != -1)
                {
                    Student s = cbSElev.SelectedValue as Student;

                    db.SeekPresentToPrint(checkboxes, s, fraDato.SelectedDate, tilDato.SelectedDate);
                }
            }

        }

        private void CmdClear_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }
    }
}
