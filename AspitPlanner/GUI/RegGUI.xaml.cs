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
            using (DBCon db = new DBCon())
            {
                CBHold.DataContext = db.GetHold();
                CBModul1.DataContext = db.GetAbcentTypes();
                CBModul2.DataContext = db.GetAbcentTypes();
                CBModul3.DataContext = db.GetAbcentTypes();
                CBModul4.DataContext = db.GetAbcentTypes();
                Elever.DataContext = db.Students.ToList();
                
            }
        }

        private void CBHold_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            if(CBHold.SelectedIndex != -1)
            {                
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
                    Elever.DataContext = db.Students.ToList();
                }
            }
        }

        private void CmdClear_Click(object sender, RoutedEventArgs e)
        {
            CBHold.SelectedIndex = -1;
        }
    }
}
