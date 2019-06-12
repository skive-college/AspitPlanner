using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Models
{
    public class Type
    {
        public int ID { get; set; }
        [StringLength(50)]
        public string TypeName { get; set; }
        public int CatID { get; set; }
    }
}
