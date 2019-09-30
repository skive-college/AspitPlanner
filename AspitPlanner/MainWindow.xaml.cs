using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
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
        HolidayGUI hd;
        User current;
        bool notHoliday = true;
        bool notFridayFri = true;
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
                if(notHoliday && notFridayFri && DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                {
                    setTitle("Registrering");
                    MainContent.Children.Add(rg);
                    Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
                    thread.Start();


                }
                else
                {
                    MainContent.Children.Add(st);
                    setTitle("Statestik");
                    setStatus("Eleverne har fri så det er ikke muligt at registrere");
                }
                List<string> manglerIGår = SQLDB.getNotPressent(Util.getDateTimeYesterday());
                if(manglerIGår.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in manglerIGår)
                    {
                        sb.AppendLine(s);
                    }
                    MessageBox.Show(sb.ToString());
                }
                
            }
            catch (Exception ex)
            {
                FileHandler.Error(ex);
            }

        }

        public void WorkThreadFunction()
        {
            try
            {
                using (DBCon db = new DBCon())
                {
                    
                    SQLDB.CreateStudentForToday(db.Students.ToList());
                }
            }
            catch (Exception ex)
            {
                // log errors
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
            st = new statistic(this);
            ug = new UserGUI();
            hd = new HolidayGUI();
        }
       
        
        private void generateMenu()
        {
            MenuItem menu = new MenuItem();
            menu.Header = "Menu";
            menu.Height = 25;
            
            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                using (DBCon db = new DBCon())
                {

                    notHoliday = db.notHoliday();
                    notFridayFri = !Util.validerFredagLigeUge(DateTime.Now);
                    if (notHoliday && notFridayFri)
                    {
                        MenuItem Reg = new MenuItem();

                        Reg.Header = "Registrere";
                        Reg.Click += RegGUI_Click;
                        menu.Items.Add(Reg);

                        MenuItem Manglede = new MenuItem();
                        Manglede.Header = "Manglede elever";
                        Manglede.Click += PLRegGUI_Click;
                        menu.Items.Add(Manglede);
                    }
                    else
                    {
                        setStatus("Eleverne har fri eller ferie så det er ikke muligt at registrere");
                    }
                }
            }
            if (current.UserRole == 1)
            {
                MenuItem Type = new MenuItem();
                Type.Header = "Type administration";
                Type.Click += TypeAdmin_Click;
                menu.Items.Add(Type);

                MenuItem Elev = new MenuItem();
                Elev.Header = "Elev administration";
                Elev.Click += StudentAdmin_Click;
                menu.Items.Add(Elev);

                MenuItem users = new MenuItem();
                users.Header = "Bruger administration";
                users.Click += Users_Click;
                menu.Items.Add(users);

                MenuItem holiday = new MenuItem();
                holiday.Header = "Ferie administration";
                holiday.Click += Holiday_Click;
                menu.Items.Add(holiday);
            }
            MenuItem Aftaler = new MenuItem();
            Aftaler.Header = "Aftaler administration";
            Aftaler.Click += Appointment_Click;
            menu.Items.Add(Aftaler);

            MenuItem Statestik = new MenuItem();
            Statestik.Header = "Statestik";
            Statestik.Click += StatisticGUI_Click;
            menu.Items.Add(Statestik);

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
        private void Holiday_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            hd.load();
            MainContent.Children.Add(hd);
            setTitle("ferie admin");
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
