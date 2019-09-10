using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AspitPlanner.Helpers
{
    public class DBCon : DbContext
    {
        //private static readonly string con = "DBCon";
        private static readonly string con = "Local";

        public DBCon() : base(ConfigurationManager.ConnectionStrings[con].ConnectionString) { }
        
        public DbSet<Student> Students { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Present> Presents { get; set; }
        public DbSet<Models.RegistrationType> Types { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet <ModulNote> ModulNotes { get; set; }

        public DbSet<Holiday> Holidays { get; set; }
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

        public List<AppointmentStudent> getAllPresents(int studentID)
        {
            List<AppointmentStudent> retur = new List<AppointmentStudent>();
            using (DBCon db = new DBCon())
            {
                var quary = (from a in db.Appointments
                              join s in db.Students on a.StudentID equals s.ID
                              select new AppointmentStudent { ID = a.ID, StudentID = s.ID, Name = s.Name,
                                                            Team = s.Team, FromeDate = a.FromeDate,
                                                            ToDate = a.ToDate, Day = a.Day, Modules = a.Modules,
                                                            Info = a.Info
                              })
                               .ToList();

                if(studentID != -1)
                {
                    quary = quary.Where(aps => aps.StudentID == studentID).ToList();
                }
                retur = quary;
            }
            return retur;
        }

        public bool notHoliday()
        {
            
            bool retur = true;
            using (DBCon db = new DBCon())
            {
                var quary = from h in db.Holidays select h;

                foreach(Holiday h in quary.ToList())
                {
                    if(DateTime.Now >= h.From && DateTime.Now <= h.Too)
                    {
                        retur = false;
                        break;
                    }
                }
            }

            return retur;
        }

        public List<StudentStatistic> getStatistics(Student student, DateTime? fra, DateTime? til)
        {
            
            List<StudentStatistic> retur = new List<StudentStatistic>();
            using (DBCon db = new DBCon())
            {
                if(student != null)
                {
                    var quary = from p in db.Presents
                                where p.StudentID == student.ID
                                select p;

                    if (fra != null)
                    {
                        fra = getDateTime(fra);
                        quary = quary.Where(x => x.Date >= fra);
                    }
                    if (til != null)
                    {
                        til = getDateTime(til);
                        quary = quary.Where(x => x.Date <= til);
                    }
                    List<Present> pre = quary.ToList();
                    var typeIDS = from t in db.Types
                                  join c in db.Categorys
                                  on t.CatID equals c.ID
                                  where c.CategoryName == "fravær"
                                  select t;
                    List<int> typeID = new List<int>();
                    foreach(RegistrationType rt in typeIDS.ToList())
                    {
                        typeID.Add(rt.ID);
                    }
                    int fremødt = CalcProcent(pre, typeID);
                    var aftale = from t in db.Types
                                 where t.TypeName == "fri"
                                 select t;
                    typeID.Remove(aftale.ToList()[0].ID);
                    
                    int fremødtMedAftale = CalcProcent(pre, typeID);
                    retur.Add(new StudentStatistic() { StudentName = student.Name + " " +student.Team, Fremøde = fremødt, FremødeUdenAftale = fremødtMedAftale });

                }
                else
                {
                    var students = from S in db.Students select S;

                    foreach(Student st in students.ToList())
                    {
                        var quary = from p in db.Presents
                                    where p.StudentID == st.ID
                                    select p;

                        if (fra != null)
                        {
                            fra = getDateTime(fra);
                            quary = quary.Where(x => x.Date >= fra);
                        }
                        if (til != null)
                        {
                            til = getDateTime(til);
                            quary = quary.Where(x => x.Date <= til);
                        }
                        List<Present> pre = quary.ToList();
                        var typeIDS = from t in db.Types
                                      join c in db.Categorys
                                      on t.CatID equals c.ID
                                      where c.CategoryName == "fravær"
                                      select t;
                        List<int> typeID = new List<int>();
                        foreach (RegistrationType rt in typeIDS.ToList())
                        {
                            typeID.Add(rt.ID);
                        }
                        int fremødt = CalcProcent(pre, typeID);
                        var aftale = from t in db.Types
                                     where t.TypeName == "fri"
                                     select t;
                        typeID.Remove(aftale.ToList()[0].ID);

                        int fremødtMedAftale = CalcProcent(pre, typeID);
                        retur.Add(new StudentStatistic() { StudentName = st.Name + " " + st.Team, Fremøde = fremødt, FremødeUdenAftale = fremødtMedAftale });
                    }
                }
                
            }
            
            return retur;
        }


        public void SeekPresentToPrint(List<CheckBox> checkboxes, Student student, DateTime? fra, DateTime? til)
        {
            using (DBCon db = new DBCon())
            {
                var quary = from p in db.Presents
                            where p.StudentID == student.ID
                            select p;

                if (fra != null)
                {
                    fra = getDateTime(fra);
                    quary = quary.Where(x => x.Date >= fra);
                }
                if (til != null)
                {
                    til = getDateTime(til);
                    quary = quary.Where(x => x.Date <= til);
                }
                

                List<Present> pre = quary.ToList();
                FileHandler.Print(pre, db.Types.ToList(), student);
            }
        }

        private int CalcProcent(List<Present> pre, List<int> TypeIDS)
        {
            double ialtRegistreret = pre.Count * 4;
            double fremødt = 0;
            foreach(Present p in pre)
            {
                if(!TypeIDS.Contains(p.Model1))
                {
                    fremødt++;
                }
                if (!TypeIDS.Contains(p.Model2))
                {
                    fremødt++;
                }
                if (!TypeIDS.Contains(p.Model3))
                {
                    fremødt++;
                }
                if (!TypeIDS.Contains(p.Model4))
                {
                    fremødt++;
                }
            }

            return (int)((fremødt / ialtRegistreret) * 100);
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
                        where pre.Date.Equals(today)
                        select pre;
            if(studentID != -1)
            {
                quary = quary.Where(x => x.StudentID.Equals(studentID));
            }
            if(quary != null)
            {
                return quary.FirstOrDefault() as Present;
            }
            return p;            
        }

        public List<Student> getNotPressent(DateTime today)
        {
            //todo: flyt til DBSQL
            List<Student> retur = new List<Student>();

            var quary = from pre in Presents
                            join stu in Students on pre.StudentID equals stu.ID
                            join ty in Types on pre.Model1 equals ty.ID
                            where pre.Date.Equals(today) && ty.TypeName == "Ikke set" && stu.Aktiv == true
                        select stu;
            var idag = from pre in Presents where pre.Date.Equals(today) select pre.StudentID;
            var quary2 = Students.Where(x => !idag.Contains(x.ID) && x.Aktiv == true);
            var quary3 = from pre in Presents
                        join stu in Students on pre.StudentID equals stu.ID
                        where pre.Date.Equals(today) && pre.Model1 == 0 && stu.Aktiv == true
                         select stu;

            retur = quary.ToList();
            foreach(Student s in quary2)
            {
                retur.Add(s);                
                
            }
            foreach (Student s in quary3)
            {
                retur.Add(s);
            }
            return retur;
            
        }

        public int GetAftaleFri(int id)
        {
            int i = -1;
            var quary = from ty in GetAbcentTypes()
                        where ty.ID.Equals(id)
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

        private DateTime getDateTime(DateTime? date)
        {
            DateTime d = (DateTime)date;
            d = new DateTime(d.Year, d.Month, d.Day);

            return d;
        }
    }    
}