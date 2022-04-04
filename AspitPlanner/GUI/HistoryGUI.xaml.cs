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
    /// Interaction logic for HistoryGUI.xaml
    /// </summary>
    public partial class HistoryGUI : UserControl
    {
        public HistoryGUI()
        {
            InitializeComponent();
        }

        public void Load() 
        {
            List<Present> missing = SQLDB.GetIncompleteRegistrations(Util.getDateTime());
            List<AbsentType> list = SQLDB.GetAbcentTypes();
       
            
             var vms = missing.Select(s => new HistoryViewModel(SQLDB.GetModuleNotes(s.StudentID, s.Date).Note)
            {
                Model = s,
                Modul1 = list.Where(l => l.ID == s.Model1).FirstOrDefault(),
                Modul2 = list.Where(l => l.ID == s.Model2).FirstOrDefault(),
                Modul3 = list.Where(l => l.ID == s.Model3).FirstOrDefault(),
                Modul4 = list.Where(l => l.ID == s.Model4).FirstOrDefault(),
                
                Types = list }).OrderByDescending(vm => vm.Model.Date).ThenBy(vm => vm.Model.StudentModel.Team);
            RegistreringsListView.ItemsSource = vms;
        }

        private void OpdaterKnap_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }
    }

    public class HistoryViewModel
    {

        public HistoryViewModel(string note)
        {
            _note = note;
        }
        public List<AbsentType> Types { get; set; }
        public Present Model { get; set; }

        private AbsentType _modul1;
        public AbsentType Modul1 
        {
            get => _modul1;
            set
            {
                if (_modul1 != value)
                {
                    _modul1 = value;
                    if (_modul1 != null)
                    {
                        Model.Model1 = _modul1.ID;
                        SQLDB.UpdatePresent(Model);
                    }
                }
            } 
        }

        private AbsentType _modul2;
        public AbsentType Modul2
        {
            get => _modul2;
            set
            {
                if (_modul2 != value)
                {
                    _modul2 = value;
                    if (_modul2 != null)
                    {
                        Model.Model2 = _modul2.ID;
                        SQLDB.UpdatePresent(Model);
                    }
                }
            }
        }

        private AbsentType _modul3;
        public AbsentType Modul3
        {
            get => _modul3;
            set
            {
                if (_modul3 != value)
                {
                    _modul3 = value;
                    if (_modul3 != null)
                    {
                        Model.Model3 = _modul3.ID;
                        SQLDB.UpdatePresent(Model);
                    }
                }
            }
        }

        private AbsentType _modul4;
        public AbsentType Modul4
        {
            get => _modul4;
            set
            {
                if (_modul4 != value)
                {
                    _modul4 = value;
                    if (_modul4 != null)
                    {
                        Model.Model4 = _modul4.ID;
                        SQLDB.UpdatePresent(Model);
                    }
                }
            }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set
            {
                if (_note != value)
                {
                    _note = value;
                    if (_note != null)
                    {
                        SQLDB.AddOrUpdateModulNote(new ModulNote { Date = Model.Date, Note = value, StudentID = Model.StudentID });
                    }
                }; 
            }
        }

    }
}
