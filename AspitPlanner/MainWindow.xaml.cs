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
using AspitPlanner.GUI;

namespace AspitPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RegGUI rg = new RegGUI();
        StudentAdmin sa = new StudentAdmin();
        TypeAdmin ta = new TypeAdmin();
        AppointmentGUI ap = new AppointmentGUI();
        String titel = "Aspit Planner";
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Children.Add(rg);
            setTitle("Registrering");
        }

        private void setTitle(String page)
        {

            this.Title = $"{titel} - {page}";
        }

        private void StudentAdmin_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            sa.load();
            MainContent.Children.Add(sa);
            setTitle("Elev");
        }

        private void TypeAdmin_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            ta.load();
            MainContent.Children.Add(ta);
            setTitle("Type admin");
        }

        private void RegGUI_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            rg.load();
            MainContent.Children.Add(rg);
            setTitle("Registrering");
        }

        private void Appointment_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            ap.load();
            MainContent.Children.Add(ap);
            
            setTitle("Aftaler");
        }
    }
}
