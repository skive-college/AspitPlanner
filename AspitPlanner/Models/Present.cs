using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Models
{
    public class Present
    {
        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }
        [Key]
        [Column(Order = 2)]
        public int StudentID { get; set; }
        public string Model1 { get; set; }
        public string Model2 { get; set; }
        public string Model3 { get; set; }
        public string Model4 { get; set; }

    }
}
