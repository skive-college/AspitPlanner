using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Helpers
{
    public class DBCon : DbContext
    {
        public static readonly string con = "Local";

        public DBCon() : base(ConfigurationManager.ConnectionStrings[con].ConnectionString) { }
        
        public DbSet<Student> Students { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Present> Presents { get; set; }
        public DbSet<Models.Type> Types { get; set; }

               
        public List<Category> 
        {
            var balance = (from a in DBCon.Types
                           join c in context.Clients on a.UserID equals c.UserID
                           where c.ClientID == yourDescriptionObject.ClientID
                           select a.Balance)
              .SingleOrDefault();

        }
    }    
}
