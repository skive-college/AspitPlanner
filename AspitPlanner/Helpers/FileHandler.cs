using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspitPlanner.Models;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

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

                xlWorkSheet.Cells[row, 3] = getTypeName(typer, p.Model1);
                xlWorkSheet.Cells[row, 4] = getTypeName(typer, p.Model2);
                xlWorkSheet.Cells[row, 5] = getTypeName(typer, p.Model3);
                xlWorkSheet.Cells[row, 6] = getTypeName(typer, p.Model4);

                row++;
            }

            string path = "";
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
           
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = dialog.FileName+ "\\";
            }

            string fileName = $"{student.Name}_{student.Team}_{DateTime.Now.ToShortDateString()}.xls";

            xlWorkBook.SaveAs(path + fileName, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

        }
        private static string getTypeName(List<RegistrationType> typer, int i)
        {
            string name = "FEJL";

            RegistrationType t = (typer.Where(x => x.ID == i).FirstOrDefault());
            if (t != null)
                name = t.TypeName;

            return name;
        }
    }
}
