using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Helpers
{
    public class SQLDB
    {
        private static readonly string con = "DBCon";
        //private static readonly string con = "Local";
        private static readonly string conString = ConfigurationManager.ConnectionStrings[con].ConnectionString;

        public static void CreateStudentForToday(List<Student> students)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();
                foreach (Student s in students)
                {
                    if (s.Aktiv == true)
                    {
                        SqlCommand cmd;
                        String sql = "Insert into Presents values(@Date,@sId,@m1,@m2,@m3,@m4)";
                        cmd = new SqlCommand(sql, cnn);
                        DateTime today = Util.getDateTime();
                        cmd.Parameters.AddWithValue("Date", today);
                        cmd.Parameters.AddWithValue("sId", s.ID);

                        List<Appointment> app = getAppStud(s.ID);
                        if (app.Count != 0)
                        {
                            foreach (Appointment a in app)
                            {
                                if (a.FromeDate <= today && today <= a.ToDate && a.Day.Contains(today.DayOfWeek.ToString()))
                                {
                                    string[] moduler = a.Modules.Split(',');
                                    foreach (string st in moduler)
                                    {
                                        if (st == "M1")
                                        {
                                            cmd.Parameters.AddWithValue("m1", a.RegistrationTypeID);
                                        }

                                        if (st == "M2")
                                        {
                                            cmd.Parameters.AddWithValue("m2", a.RegistrationTypeID);
                                        }
                                        if (st == "M3")
                                        {
                                            cmd.Parameters.AddWithValue("m3", a.RegistrationTypeID);
                                        }
                                        if (st == "M4")
                                        {
                                            cmd.Parameters.AddWithValue("m4", a.RegistrationTypeID);
                                        }
                                    }
                                }
                            }
                        }
                        if (!cmd.Parameters.Contains("m1"))
                            cmd.Parameters.AddWithValue("m1", 0);
                        if (!cmd.Parameters.Contains("m2"))
                            cmd.Parameters.AddWithValue("m2", 0);
                        if (!cmd.Parameters.Contains("m3"))
                            cmd.Parameters.AddWithValue("m3", 0);
                        if (!cmd.Parameters.Contains("m4"))
                            cmd.Parameters.AddWithValue("m4", 0);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                
            }
        }

        public static void SetInactiv(Student s)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();

                SqlCommand cmd;
                String sql = "Update Students SET Aktiv='false' WHERE ID = @ID ";
                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("ID", s.ID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
        }

        private static List<Appointment> getAppStud(int id)
        {

            List<Appointment> retur = new List<Appointment>();
            SqlConnection cnn = new SqlConnection(conString);
            cnn.Open();
            SqlCommand cmd;
            String sql = "SELECT * FROM Appointments where StudentID = @id";
            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("id", id);

            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    Appointment a = new Appointment();
                    a.ID = int.Parse(oReader["ID"].ToString());
                    a.StudentID = int.Parse(oReader["StudentID"].ToString());
                    a.FromeDate = DateTime.Parse(oReader["FromeDate"].ToString());
                    a.ToDate = DateTime.Parse(oReader["ToDate"].ToString());
                    a.Day = oReader["Day"].ToString();
                    a.Modules = oReader["Modules"].ToString();
                    a.Info = oReader["Info"].ToString();
                    a.RegistrationTypeID = int.Parse(oReader["RegistrationTypeID"].ToString());

                    retur.Add(a);
                }
            }

            return retur;
        }
        public static List<ChartValue> GetDifrences(int sID, DateTime? fra, DateTime? til)
        {
            List<ChartValue> retur = new List<ChartValue>();
            SqlConnection cnn = new SqlConnection(conString);
            List<regType> regtypes = getAllTypes();
            int[] counts = new int[regtypes.Count];
            cnn.Open();

            SqlCommand cmd;
            String sql = "SELECT * FROM Presents p, Students s where p.StudentID = @StudentID AND p.StudentID = s.ID";
            if (fra != null)
            {
                sql += " AND Date >= @fra";
            }
            if (til != null)
            {
                sql += " AND Date <= @til";
            }
            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("StudentID", sID);
            if (fra != null)
            {
                cmd.Parameters.AddWithValue("fra", fra);
            }
            if (til != null)
            {
                cmd.Parameters.AddWithValue("til", til); ;
            }
            int ialt = 0;
            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    for (int i = 0; i < regtypes.Count; i++)
                    {
                        if (int.Parse(oReader["Model1"].ToString()) == regtypes[i].ID)
                            counts[i]++;
                        if (int.Parse(oReader["Model2"].ToString()) == regtypes[i].ID)
                            counts[i]++;
                        if (int.Parse(oReader["Model3"].ToString()) == regtypes[i].ID)
                            counts[i]++;
                        if (int.Parse(oReader["Model4"].ToString()) == regtypes[i].ID)
                            counts[i]++;

                        ialt++;
                    }
                }

            }
            int medfri = 0;
            int udenfri = 0;
            int mødt = 0;
            int mødtudenInaktiv = 0;
            for (int i = 0; i < regtypes.Count; i++)
            {
                if (regtypes[i].CatID == 1)
                {
                    medfri += counts[i];
                    if (regtypes[i].TypeName != "Fri")
                        udenfri += counts[i];
                }
                if (regtypes[i].CatID == 2)
                {
                    mødt += counts[i];
                    if (regtypes[i].TypeName != "Inaktiv")
                        mødtudenInaktiv += counts[i];
                }
            }
            retur.Add(new ChartValue { ID = 1, Navn = "Fravær uden aftale", Procent = medfri });
            retur.Add(new ChartValue { ID = 2, Navn = "Fravær med aftale", Procent = udenfri });
            retur.Add(new ChartValue { ID = 3, Navn = "Mødt", Procent = mødt });
            retur.Add(new ChartValue { ID = 4, Navn = "Mødt aktiv", Procent = mødtudenInaktiv });
            return retur;
        }

        public static void deleteAppointment(int iD)
        {
            SqlConnection cnn = new SqlConnection(conString);
            cnn.Open();

            SqlCommand cmd;
            String sql = "Delete From Appointments Where ID = @ID";
           
            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("ID", iD);
            cmd.ExecuteNonQuery();
        }

        public static List<ChartValue> getData(int sID, DateTime? fra, DateTime? til)
        {
            List<ChartValue> retur = new List<ChartValue>();
            SqlConnection cnn = new SqlConnection(conString);
            List<regType> regtypes = getAllTypes();
            int[] counts = new int[regtypes.Count];
            cnn.Open();

            SqlCommand cmd;
            String sql = "SELECT * FROM Presents p, Students s where p.StudentID = @StudentID AND p.StudentID = s.ID";
            if(fra != null)
            {
                sql += " AND Date >= @fra";
            }
            if (til!= null)
            {
                sql += " AND Date <= @til";
            }

            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("StudentID", sID);
            if (fra != null)
            {
                cmd.Parameters.AddWithValue("fra", fra);
            }
            if (til != null)
            {
                cmd.Parameters.AddWithValue("til", til); ;
            }
            
            int ialt = 0;
            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    for (int i = 0; i < regtypes.Count; i++)
                    {
                        if (int.Parse(oReader["Model1"].ToString()) == regtypes[i].ID)
                            counts[i]++;
                        if (int.Parse(oReader["Model2"].ToString()) == regtypes[i].ID)
                            counts[i]++;
                        if (int.Parse(oReader["Model3"].ToString()) == regtypes[i].ID)
                            counts[i]++;
                        if (int.Parse(oReader["Model4"].ToString()) == regtypes[i].ID)
                            counts[i]++;

                        ialt++;
                    }
                }

            }
            for (int i = 0; i < regtypes.Count; i++)
            {
                retur.Add(new ChartValue { ID = regtypes[i].ID, Navn = regtypes[i].TypeName, Procent = counts[i] });
            }
            return retur;
        }


        private static List<regType> getAllTypes()
        {
            List<regType> retur = new List<regType>();
            SqlConnection cnn = new SqlConnection(conString);

            cnn.Open();

            SqlCommand cmd;
            String sql = "SELECT * FROM RegistrationTypes";

            cmd = new SqlCommand(sql, cnn);
            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    retur.Add(
                        new regType
                        {
                            ID = int.Parse(oReader["ID"].ToString()),
                            TypeName = oReader["TypeName"].ToString(),
                            CatID = int.Parse(oReader["CatID"].ToString())

                        });
                }

            }
            return retur;
        }

        public static string getTypeName(int ID)
        {
            string retur = "";
            SqlConnection cnn = new SqlConnection(conString);

            cnn.Open();

            SqlCommand cmd;
            String sql = "SELECT * FROM RegistrationTypes where ID = @ID ";

            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("ID", ID);
            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    retur = oReader["TypeName"].ToString();
                }

            }
            return retur;
        }

        public static List<string> getNotPressent(DateTime _date)
        {
            List<string> retur = new List<string>();
            SqlConnection cnn = new SqlConnection(conString);

            cnn.Open();

            SqlCommand cmd;
            String sql = "select * from Presents p, students s where p.studentID = s.ID And (Model1 = 0 OR Model2 = 0 OR Model3 = 0 OR Model4 = 0 ) AND Date < @Date";

            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("Date", _date);
            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    string s = "";
                    s += oReader["Name"].ToString() + " ";
                    s += oReader["Team"].ToString() + " ";
                    s += oReader["Date"].ToString() + " ";
                    s += "m1 = " + oReader["Model1"].ToString();
                    s += "m2 = " + oReader["Model2"].ToString();
                    s += "m3 = " + oReader["Model3"].ToString();
                    s += "m4 = " + oReader["Model4"].ToString();
                    retur.Add(s);
                }

            }
            return retur;
        }


    }
}
