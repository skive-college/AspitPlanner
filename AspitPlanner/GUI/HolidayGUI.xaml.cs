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
    /// Interaction logic for HolidayGUI.xaml
    /// </summary>
    public partial class HolidayGUI : UserControl
    {
        public HolidayGUI()
        {
            InitializeComponent();
        }

        private void CmdOpret_Click(object sender, RoutedEventArgs e)
        {
            if(dpFrom.Text != "" && dpToo.Text != "")
            {
                Holiday h = new Holiday() { From = (DateTime)dpFrom.SelectedDate, Too = (DateTime)dpToo.SelectedDate };
                
                SQLDB.AddHoliday(h);
                MainWindow.setStatus("Ferie oprettet");
                load();
            }
            
        }

        public void load()
        {
            holidayList.DataContext = SQLDB.GetHolidays();
            
            
        }

        private void CmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if(holidayList.SelectedIndex != -1)
            {
                Holiday h = holidayList.SelectedValue as Holiday;
                SQLDB.RemoveHoliday(h);
                load();
            }
        }
    }
}
