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
using AspitPlanner.Helpers;
using AspitPlanner.Models;

namespace AspitPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static TextBox lbStatus;
        RegGUI rg;
        StudentAdmin sa;
        TypeAdmin ta;
        AppointmentGUI ap;
        PLRegGUI pl;
        String titel;
        statistic st;
        UserGUI ug;
        User current;
        public MainWindow()
        {
            try
            {
                
                InitializeComponent();
                lbStatus = lblStatus;
                LoginGUI Login = new LoginGUI();
                Login.ShowDialog();

                setTitle("Registrering");
                if (Login.DialogResult == true)
                {
                    current = Login.GetUser();
                    generateMenu();
                    setStatus("velkommen " + current.Usernane);
                }
                else if (Login.DialogResult == false)
                {
                    this.Close();
                }
                LoadContent();
                setTitle("Registrering");
                MainContent.Children.Add(rg);
            }
            catch (Exception ex)
            {
                FileHandler.Error(ex);
            }

        }

        private void LoadContent()
        {
            rg = new RegGUI();
            sa = new StudentAdmin();
            ta = new TypeAdmin();
            ap = new AppointmentGUI();
            pl = new PLRegGUI();
            titel = "Aspit Planner";
            st = new statistic();
            ug = new UserGUI();
        }

        private void generateMenu()
        {
            MenuItem menu = new MenuItem();
            menu.Header = "Menu";
            menu.Height = 25;

            MenuItem Reg = new MenuItem();
            Reg.Header = "Registrere";
            Reg.Click += RegGUI_Click;
            menu.Items.Add(Reg);

            MenuItem Elev = new MenuItem();
            Elev.Header = "Elev administration";
            Elev.Click += StudentAdmin_Click;
            menu.Items.Add(Elev);

            MenuItem Aftaler = new MenuItem();
            Aftaler.Header = "Aftaler administration";
            Aftaler.Click += Appointment_Click;
            menu.Items.Add(Aftaler);

            MenuItem Manglede = new MenuItem();
            Manglede.Header = "Manglede elever";
            Manglede.Click += PLRegGUI_Click;
            menu.Items.Add(Manglede);

            MenuItem Statestik = new MenuItem();
            Statestik.Header = "Statestik";
            Statestik.Click += StatisticGUI_Click;
            menu.Items.Add(Statestik);

            if (current.UserRole == 1)
            {
                MenuItem Type = new MenuItem();
                Type.Header = "Type administration";
                Type.Click += TypeAdmin_Click;
                menu.Items.Add(Type);

                MenuItem users = new MenuItem();
                users.Header = "Bruger administration";
                users.Click += Users_Click;
                menu.Items.Add(users);
            }

            MainMenu.Items.Add(menu);
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            
            MainContent.Children.Add(ug);
            setTitle("Bruger administration");
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

        private void PLRegGUI_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            pl.load();
            MainContent.Children.Add(pl);
            setTitle("Manglede elever");
        }

        private void StatisticGUI_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            st.Load();
            MainContent.Children.Add(st);
            setTitle("Statestik");
        }

        public static void setStatus(String msg)
        {
            lbStatus.Text = msg;
        }
    }
}
