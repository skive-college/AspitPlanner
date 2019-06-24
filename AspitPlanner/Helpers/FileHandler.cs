using System;
using System.Collections.Generic;
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
                xlWorkSheet.Cells[row, 3] = (typer.Where(x => x.ID == p.Model1).FirstOrDefault()).TypeName;
                xlWorkSheet.Cells[row, 4] = (typer.Where(x => x.ID == p.Model2).FirstOrDefault()).TypeName;
                xlWorkSheet.Cells[row, 5] = (typer.Where(x => x.ID == p.Model3).FirstOrDefault()).TypeName;
                xlWorkSheet.Cells[row, 6] = (typer.Where(x => x.ID == p.Model4).FirstOrDefault()).TypeName;

                row++;

            }

            xlWorkBook.SaveAs("c:\\temp\\csharp-Excel.xls", XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
;
        }
    }
}
