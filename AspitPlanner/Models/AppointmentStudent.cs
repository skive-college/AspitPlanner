using AspitPlanner.Models;
using System;

namespace AspitPlanner.Models
{
    public class AppointmentStudent
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public string Name { get; set; }
        public string Team { get; set; }
        public DateTime FromeDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Day { get; set; }
        public string Modules { get; set; }
        public string Info { get; set; }
    }
}