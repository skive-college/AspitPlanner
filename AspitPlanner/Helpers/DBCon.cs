using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = AspitPlanner.Models.Type;

namespace AspitPlanner.Helpers
{
    public class DBCon : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Present> Presents{ get; set; }
        public DbSet<Type> Types{ get; set; }

    }
}
