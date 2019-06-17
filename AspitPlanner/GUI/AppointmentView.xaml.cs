using AspitPlanner.Helpers;
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
        }

        private void RegistrationsGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "ID" || e.Column.Header.ToString() == "StudentID")
                e.Cancel = true;
        }

        private void load()
        {
            using(DBCon db = new DBCon())
            {
                RegistrationsGrid.DataContext = db.getAllPresents(-1);
            }
        }
    }
}
