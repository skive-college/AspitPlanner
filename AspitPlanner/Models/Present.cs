using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Models
{
    public class Present
    {
        internal const string TABLE_NAME = "Presents";
        internal const string STUDENT_ID = "StudentId";
        internal const string DATE = "Date";
        internal const string MODULE_1 = "Model1";
        internal const string MODULE_2 = "Model2";
        internal const string MODULE_3 = "Model3";
        internal const string MODULE_4 = "Model4";

        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }
        [Key]
        [Column(Order = 2)]
        public int StudentID { get; set; }
        public int Model1 { get; set; }
        public int Model2 { get; set; }
        public int Model3 { get; set; }
        public int Model4 { get; set; }
        public Student StudentModel { get; set; }
        internal static Present MapFromReader(SqlDataReader reader)
        {
            Present p = new Present();
            p.Date = (DateTime)reader[DATE];
            p.Model1 = (int)reader[MODULE_1];
            p.Model2 = (int)reader[MODULE_2];
            p.Model3 = (int)reader[MODULE_3];
            p.Model4 = (int)reader[MODULE_4];
            p.StudentID = (int)reader[STUDENT_ID];
            return p;
        }
    }
}
