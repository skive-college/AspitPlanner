﻿using AspitPlanner.Helpers;
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
    /// Interaction logic for statistic.xaml
    /// </summary>
    public partial class statistic : UserControl
    {
        List<CheckBox> checkboxes = new List<CheckBox>();
        public statistic()
        {
            InitializeComponent();
            loadStudents();
            CreateCheckBoxes();
        }

        private void loadStudents()
        {
            using (DBCon db = new DBCon())
            {
                cbSElev.DataContext = db.Students.ToList();
            }
        }

        private void CreateCheckBoxes()
        {
            using(DBCon db = new DBCon())
            {
                List<Category> cats = db.Categorys.ToList();
                for (int i = 0; i < cats.Count; i++)
                {
                    StackPanel p = new StackPanel();
                    
                    PanelGrid.Children.Add(p);
                    Grid.SetColumn(p, i);
                    int id = cats[i].ID;
                    var quary = from t in db.Types
                                where t.CatID == id
                                select t;

                    List<RegistrationType> typer = quary.ToList();
                    for(int j = 0; j < typer.Count; j++)
                    {
                        CheckBox c = new CheckBox();
                        c.Checked += C_Checked;
                        c.Content = typer[j].TypeName;
                        checkboxes.Add(c);
                        p.Children.Add(c);
                    }
                }
            }
        }

        private void C_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox c = sender as CheckBox;

        }

        private void CmdSeek_Click(object sender, RoutedEventArgs e)
        {
            using(DBCon db = new DBCon())
            {
                if(cbSElev.SelectedIndex != -1)
                {
                    Student s = cbSElev.SelectedValue as Student;


                    db.SeekPresent(checkboxes, s.ID);
                }
            }
            
            
        }

    }
}
