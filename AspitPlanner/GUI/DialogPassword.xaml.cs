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
    /// Interaction logic for DialogPassword.xaml
    /// </summary>
    public partial class DialogPassword : Window
    {
        public string Password { get; private set; }
        public DialogPassword()
        {
            InitializeComponent();
            this.Closing += MyWindow_Closing;
        }

        void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult == null)
            {
                e.Cancel = true;
            }
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {

            Password = txtPassword.Password;

            this.DialogResult = true;
        }
    }
}
