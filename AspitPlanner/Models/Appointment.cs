﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Models
{
    public class Appointment
    {
        public int ID { get; set; }
        
        public int StudentID { get; set; }
        public DateTime FromeDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Day { get; set; }
        public string Modules { get; set; }
        public string Info { get; set; }
}
}
