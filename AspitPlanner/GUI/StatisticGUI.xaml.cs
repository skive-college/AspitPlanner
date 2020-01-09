using AspitPlanner.Helpers;
using AspitPlanner.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
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
        Window parrent;
        public statistic(Window _parrent)
        {
            parrent = _parrent;
            InitializeComponent();
            loadStudents();
        }

        public void Load()
        {
            loadStudents();
            tilDato.Text = "";
            fraDato.Text = "";
            tilDato.SelectedDate = null;
            fraDato.SelectedDate = null;

            mcChart.Title = "";
            ((PieSeries)mcChart.Series[0]).ItemsSource = new List<ChartValue>();
            frChart.Title = "";
            ((ColumnSeries)frChart.Series[0]).ItemsSource = new List<ChartValue>();


        }
        private void loadStudents()
        {
            using (DBCon db = new DBCon())
            {
                cbSElev.DataContext = db.Students.Where(s => s.Aktiv == true).OrderBy(s => s.Name).ToList();
                seek();
            }
        }

      

        private void C_Checked(object sender, RoutedEventArgs e)
        {
            seek();
        }

        private void seek()
        {
            if (cbSElev.SelectedIndex != -1)
            {
                int antalModulerIalt = 0;
                Student student = (cbSElev.SelectedItem as Student);

                
                List<ChartValue> li = SQLDB.getData(student.ID,fraDato.SelectedDate,tilDato.SelectedDate);

                List<ChartValue> fr = SQLDB.GetDifrences(student.ID, fraDato.SelectedDate, tilDato.SelectedDate);

                foreach (ChartValue cv in li)
                {
                    antalModulerIalt += cv.Procent;
                }


                mcChart.Title = student.Name + " moduler ialt = " + antalModulerIalt;
                mcChart.Palette = Util.MakePalette(li);
                ((PieSeries)mcChart.Series[0]).ItemsSource = li;

                frChart.Title = student.Name + " moduler ialt = " + antalModulerIalt;
                ((ColumnSeries)frChart.Series[0]).ItemsSource = fr;

            }

        }

        private void CbSElev_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            seek();            
        }

        private void FraDato_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            seek();
        }

        private void TilDato_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            seek();
        }

        private void CmdClear_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (cbSElev.SelectedIndex != -1)
            {

                Student student = (cbSElev.SelectedItem as Student);
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)parrent.Width, (int)parrent.Height, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(this);

                string PicPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Picture");
                if (!Directory.Exists(PicPath))
                    Directory.CreateDirectory(PicPath);

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));

                string alt = "alt";

                if(fraDato.Text != "" && tilDato.Text != "")
                {
                    alt = fraDato.Text + " " + tilDato.Text;
                }
                else if(fraDato.Text != "")
                {
                    alt = fraDato.Text;
                }
                else if (tilDato.Text != "")
                {
                    alt = tilDato.Text;
                }

                string filePath = System.IO.Path.Combine(PicPath, string.Format("{0} {1}.png", (student.Name + " " + student.Team), alt));
                using (Stream stream = File.Create(filePath))
                {
                    encoder.Save(stream);
                }
                MainWindow.setStatus("billede gemt sti : " + filePath);
            }
        }
    }
}
