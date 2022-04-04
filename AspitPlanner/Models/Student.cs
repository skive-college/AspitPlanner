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
    public class Student
    {
        public const string TABLE_NAME = "Students";
        public const string COLUMN_ID = "Id";
        public const string NAME = "Name";
        public const string TEAM = "Team";
        public const string AKTIV = "Aktiv";
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        public string Team { get; set; }

        public bool Aktiv { get; set; }

        internal static Student MapFromReader(SqlDataReader reader)
        {
            Student student = new Student();
            student.ID = (int)reader[COLUMN_ID];
            student.Name = (string)reader[NAME];
            student.Team = (string)reader[TEAM];
            student.Aktiv = (bool)reader[AKTIV];
            return student;
        }
    }
}
