using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Models
{
    public class User
    {
        [Key]
        public string Usernane { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
