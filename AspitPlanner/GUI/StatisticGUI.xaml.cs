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
using System.Windows.Controls.DataVisualization.Charting;
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
            if (cbSElev.SelectedIndex != -1)
            {
                int antalModulerIalt = 0;
                Student student = (cbSElev.SelectedItem as Student);

                
                List<ChartValue> li = SQLDB.getData(student.ID,fraDato.SelectedDate,tilDato.SelectedDate);

                List<ChartValue> fr = SQLDB.GetDifrences(student.ID, fraDato.SelectedDate, tilDato.SelectedDate);

                foreach (ChartValue cv in li)
                {
                    antalModulerIalt += cv.Procent;
                }


                mcChart.Title = student.Name + " moduler ialt = " + antalModulerIalt;
                mcChart.Palette = Util.MakePalette(li);
                ((PieSeries)mcChart.Series[0]).ItemsSource = li;

                frChart.Title = student.Name + " moduler ialt = " + antalModulerIalt;
                ((ColumnSeries)frChart.Series[0]).ItemsSource = fr;

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

        private void CmdClear_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }
    }
}
