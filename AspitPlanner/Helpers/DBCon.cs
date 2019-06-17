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
        private static readonly string con = "Local";

        public DBCon() : base(ConfigurationManager.ConnectionStrings[con].ConnectionString) { }
        
        public DbSet<Student> Students { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Present> Presents { get; set; }
        public DbSet<Models.RegistrationType> Types { get; set; }

               
        public List<AbsentType> GetAbcentTypes() 
        {
            List<AbsentType> retur = new List<AbsentType>();
            using (DBCon db = new DBCon())
            {
                var Abcent = (from a in db.Types
                              join c in db.Categorys on a.CatID equals c.ID
                              orderby c.ID descending
                              select new AbsentType { ID = a.ID, TypeName = a.TypeName, CatName = c.CategoryName })
                               .ToList();
                retur = Abcent;
            }
            return retur;

        }

        public List<Student> GetHold()
        {
            List<Student> liste = new List<Student>();
            using (DBCon db = new DBCon())
            { 
                liste = db.Students.ToList();
                liste = liste.GroupBy(test => test.Team)
                       .Select(grp => grp.First())
                       .ToList();
                
            }
            return liste;
        }

        public List<Student> GetStundentsOnTeam(string Team)
        {
            List<Student> liste = new List<Student>();
            using (DBCon db = new DBCon())
            {
                var quarry = from s in Students
                             where s.Team == Team
                             select s;

                liste = quarry.ToList();
            }
            return liste;
        }

        public Present getPressent(DateTime today, int studentID)
        {
            Present p = null;

            var quary = from pre in Presents
                        where pre.StudentID.Equals(studentID) && pre.Date.Equals(today)
                        select pre;
            if(quary != null)
            {
                return quary.FirstOrDefault() as Present;
            }
            return p;            
        }

        public int GetAftaleFri()
        {
            int i = -1;
            var quary = from ty in GetAbcentTypes()
                        where ty.TypeName.Equals("fri")
                        select ty;


            for (int ind = 0; ind < GetAbcentTypes().Count; ind++)
            {
                if (GetAbcentTypes()[ind].ID == (quary.FirstOrDefault()).ID)
                {
                    i = ind;
                    break;
                }
                    
            }


            return i;
        }
    }    
}