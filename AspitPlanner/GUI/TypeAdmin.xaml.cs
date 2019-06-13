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

        private void load()
        {
            Clear();
            using (DBCon db = new DBCon())
            {
                CbType.DataContext = db.Categorys.ToList();
            }
            
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

                using(DBCon db = new DBCon())
                {
                    db.Categorys.Add(c);
                    db.SaveChanges();
                }
            }
            load();
        }

        private void CmdOpretType_Click(object sender, RoutedEventArgs e)
        {

            if (txtNavn.Text != "")
            {
                Models.Type t = new Models.Type();
                t.TypeName= txtNavn.Text;
                int cat = (CbType.SelectedValue as Category).ID;

                t.CatID = cat;
                using (DBCon db = new DBCon())
                {
                    db.Types.Add(t);
                    db.SaveChanges();
                }
            }

        }
    }
}
