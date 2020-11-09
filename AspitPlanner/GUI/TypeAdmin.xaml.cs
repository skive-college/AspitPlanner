using AspitPlanner.Helpers;
using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for TypeAdmin.xaml
    /// </summary>
    public partial class TypeAdmin : UserControl
    {
        public TypeAdmin()
        {
            InitializeComponent();
            load();
        }

        public void load()
        {
            Clear();
            CbType.DataContext = SQLDB.GetCategory();
            
            
        }
        private void Clear()
        {
            txtKatNavn.Text = "";
            txtNavn.Text = "";
            CbType.SelectedIndex = -1;
        }
        private void CmdOpretKategori_Click(object sender, RoutedEventArgs e)
        {
            if(txtKatNavn.Text != "")
            {
                Category c = new Category();
                c.CategoryName = txtKatNavn.Text;

                SQLDB.addCategory(c);
            }
            load();
        }

        private void CmdOpretType_Click(object sender, RoutedEventArgs e)
        {

            if (txtNavn.Text != "")
            {
                Models.RegistrationType t = new Models.RegistrationType();
                t.TypeName= txtNavn.Text;
                int cat = (CbType.SelectedValue as Category).ID;

                t.CatID = cat;
                SQLDB.AddType(t);
            }

        }
    }
}
