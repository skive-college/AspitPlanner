﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Models
{
    public class ModulNote
    {
        public static string TABLE_NAME = "ModulNotes";
        public static string STUDENT_ID = "StudentID";

        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }
        public int StudentID { get; set; }

        public string Note { get; set; }
    }
}
