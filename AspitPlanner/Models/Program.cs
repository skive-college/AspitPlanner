using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Models
{
    public class Program
    {
        public string Navn { get; set; }
        public string Sti { get; set; }

        public override string ToString()
        {
            return Navn;
        }
    }
}
