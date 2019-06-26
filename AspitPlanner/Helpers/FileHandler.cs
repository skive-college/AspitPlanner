using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspitPlanner.Models;
using Microsoft.Office.Interop.Excel;

namespace AspitPlanner.Helpers
{
    public class FileHandler
    {
        public static void Print(List<Present> pre, List<RegistrationType> typer, Student student)
        {
            Application xlApp = new Application();
            Workbook xlWorkBook;
            Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "Dato";
            xlWorkSheet.Cells[1, 2] = "Name";
            xlWorkSheet.Cells[1, 3] = "Modul 1";
            xlWorkSheet.Cells[1, 4] = "Modul 2";
            xlWorkSheet.Cells[1, 5] = "Modul 3";
            xlWorkSheet.Cells[1, 6] = "Modul 4";

            int row = 2;
            foreach (Present p in pre)
            {
                xlWorkSheet.Cells[row, 1] = p.Date.ToShortDateString();
                xlWorkSheet.Cells[row, 2] = student.Name + " " + student.Team;
                if(p.Model1 != 0)
                    xlWorkSheet.Cells[row, 3] = (typer.Where(x => x.ID == p.Model1).FirstOrDefault()).TypeName;
                if (p.Model2 != 0)
                    xlWorkSheet.Cells[row, 4] = (typer.Where(x => x.ID == p.Model2).FirstOrDefault()).TypeName;
                if (p.Model3 != 0)
                    xlWorkSheet.Cells[row, 5] = (typer.Where(x => x.ID == p.Model3).FirstOrDefault()).TypeName;
                if (p.Model4 != 0)
                    xlWorkSheet.Cells[row, 6] = (typer.Where(x => x.ID == p.Model4).FirstOrDefault()).TypeName;

                row++;

            }

            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\reports\";
            string fileName = student.Name + DateTime.Now.ToShortDateString() +  ".xls";
            System.IO.Directory.CreateDirectory(path);
            xlWorkBook.SaveAs(path+fileName, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            MainWindow.setStatus(path + fileName + " Er oprettet");

        }
        public static void Error(Exception ex)
        {
            // Create a file to write to.
            string fail = ex.ToString();
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Log.txt";
            string createText = DateTime.Now + fail + Environment.NewLine + Environment.NewLine;
            File.AppendAllText(path, createText);
        }


    }
}
