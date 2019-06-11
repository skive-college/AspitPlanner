using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Models
{
    public class Category
    {
        public int ID { get; set; }
        [StringLength(50)]
        public string CategoryName { get; set; }
    }
}
