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

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Children.Add(rg);
        }

        private void StudentAdmin_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            MainContent.Children.Add(sa);
        }

        private void TypeAdmin_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            MainContent.Children.Add(ta);
        }

        private void RegGUI_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.RemoveAt(0);
            MainContent.Children.Add(rg);
        }
    }
}
