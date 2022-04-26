using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Helpers
{
    public class SQLDB
    {
        //private static string db = "Local";
         private static string db = "DBCon";
        private static string conString = ConfigurationManager.ConnectionStrings[db].ConnectionString;



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
                                        if (st == "M1" && !cmd.Parameters.Contains("m1"))
                                        {
                                            cmd.Parameters.AddWithValue("m1", a.RegistrationTypeID);
                                        }

                                        if (st == "M2" && !cmd.Parameters.Contains("m2"))
                                        {
                                            cmd.Parameters.AddWithValue("m2", a.RegistrationTypeID);
                                        }
                                        if (st == "M3" && !cmd.Parameters.Contains("m3"))
                                        {
                                            cmd.Parameters.AddWithValue("m3", a.RegistrationTypeID);
                                        }
                                        if (st == "M4" && !cmd.Parameters.Contains("m4"))
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
                        catch (SqlException sqlEx)
                        {
                            //der for at fange alle andre fejl en primary key da den ikke er en fejl
                            if (!sqlEx.Message.StartsWith("Violation of PRIMARY KEY constraint"))
                            {
                                FileHandler.Error(sqlEx);
                            }
                        }
                        catch (Exception ex)
                        {
                            FileHandler.Error(ex);

                        }
                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
        }

        internal static void ReActivateStudent(int iD)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                string idParam = "@IdParam";
                string query = $"UPDATE {Student.TABLE_NAME} SET {Student.AKTIV} = 1 WHERE {Student.COLUMN_ID} = {idParam}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(idParam, iD);
                    command.ExecuteNonQuery();
                }
                        
            } 
        }

        internal static void DeleteStudentData(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();
                    string studentIdParam = "@Id";
                    string deleteStudentQuery = $"DELETE FROM {ModulNote.TABLE_NAME} WHERE {ModulNote.STUDENT_ID} = {studentIdParam};" +
                        $"DELETE FROM {Present.TABLE_NAME} WHERE {Present.STUDENT_ID} = {studentIdParam};" +
                        $"DELETE FROM {Appointment.TABLE_NAME} WHERE {Appointment.STUDENT_ID} ={studentIdParam};" +
                        $"DELETE FROM {Student.TABLE_NAME}  WHERE {Student.COLUMN_ID} = {studentIdParam};";
                    using (SqlCommand command = new SqlCommand(deleteStudentQuery, connection))
                    {
                        command.Parameters.AddWithValue(studentIdParam, id);
                        int res = command.ExecuteNonQuery();
                        if (res > 0)
                        {
                            //Succes
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        internal static IList<Student> GetInactiveStudents()
        {
            IList<Student> students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                string query = $"SELECT * FROM {Student.TABLE_NAME} WHERE {Student.AKTIV} = 0";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(Student.MapFromReader(reader));
                        }
                    }
                }
            }
            return students;
        }

        internal static void AddOrUpdatePresent(Present model)
        {
            throw new NotImplementedException();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                string sql = $"UPDATE ";
                using (SqlCommand command = new SqlCommand())
                {

                }
            }
        }

        public static List<UserRole> GetUserRoles()
        {
            List<UserRole> retur = new List<UserRole>();
            SqlConnection cnn = new SqlConnection(conString);
            cnn.Open();
            SqlCommand cmd;
            String sql = "SELECT * FROM UserRoles";
            cmd = new SqlCommand(sql, cnn);

            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    retur.Add(new UserRole()
                    {
                        ID = int.Parse(oReader["ID"].ToString()),
                        Name = oReader["Name"].ToString()

                    });
                }
            }
            cnn.Close();
            return retur;
        }

        public static void AddHoliday(Holiday h)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();
                String sql = "Insert into Holidays values(@From,@Too)";
                SqlCommand cmd = new SqlCommand(sql, cnn);
                DateTime today = Util.getDateTime();
                cmd.Parameters.AddWithValue("From", h.From);
                cmd.Parameters.AddWithValue("Too", h.Too);
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
        }

        public static void AddUser(User u)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();
                String sql = "Insert into Users values(@Name,@Password,@Role)";
                SqlCommand cmd = new SqlCommand(sql, cnn);
                DateTime today = Util.getDateTime();
                cmd.Parameters.AddWithValue("Name", u.Usernane);
                cmd.Parameters.AddWithValue("Password", u.Password);
                cmd.Parameters.AddWithValue("Role", u.UserRole);
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
        }

        public static void addStudent(Student s)
        {

            try
            {
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();
                String sql = "Insert into Students values(@Name,@Team,@Aktiv)";
                SqlCommand cmd = new SqlCommand(sql, cnn);
                DateTime today = Util.getDateTime();
                cmd.Parameters.AddWithValue("Name", s.Name);
                cmd.Parameters.AddWithValue("Team", s.Team);
                cmd.Parameters.AddWithValue("Aktiv", true);
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
        }

        public static void updateAppointment(AppointmentStudent apstud, DateTime? date)
        {
            SqlConnection cnn = new SqlConnection(conString);
            cnn.Open();

            SqlCommand cmd;
            String sql = "Update Appointments set ToDate = @date Where ID = @ID";

            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("ID", apstud.ID);
            if (date == null)
                cmd.Parameters.AddWithValue("date", Util.getDateTime());
            else
            {

                DateTime d = (DateTime)date;
                cmd.Parameters.AddWithValue("date", new DateTime(d.Year, d.Month, d.Day));
            }

            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public static bool UpdateUserPassword(string name, string password)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();

                SqlCommand cmd;
                String sql = "Update Users SET Password = @password WHERE Usernane = @name ";
                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("password", password);
                cmd.ExecuteNonQuery();
                cnn.Close();
                return true;
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
                return false;
            }
        }

        public static ModulNote GetModuleNotes(int sID, DateTime time)
        {
            ModulNote retur = new ModulNote();
            //retur.Note;
            retur.StudentID = sID;
            retur.Date = time;
            SqlConnection cnn = new SqlConnection(conString);
            cnn.Open();
            SqlCommand cmd;
            String sql = "SELECT * FROM ModulNotes where StudentID = @id And Date = @time";
            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("id", sID);
            cmd.Parameters.AddWithValue("time", time);

            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    retur.Note = oReader["Note"].ToString();
                }
            }
            cnn.Close();
            return retur;
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
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
        }

        public static void AddOrUpdateModulNote(ModulNote mn)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();
                SqlCommand cmd;
                String sql = "Insert Into ModulNotes Values(@date,@Sid,@note)";
                ModulNote note = GetModuleNotes(mn.StudentID, mn.Date);
                if (note.Note != null)
                {
                    sql = "Update ModulNotes set Note = @note where StudentId = @Sid And date = @date";
                }
                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("date", mn.Date);
                cmd.Parameters.AddWithValue("Sid", mn.StudentID);
                cmd.Parameters.AddWithValue("note", mn.Note);
                cmd.ExecuteNonQuery();
                cnn.Close();
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
            cnn.Close();
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
            retur.Add(new ChartValue { ID = 1, Navn = "Fravær", Procent = medfri });
            retur.Add(new ChartValue { ID = 2, Navn = "Justeret Fravær", Procent = udenfri });
            retur.Add(new ChartValue { ID = 3, Navn = "Mødt", Procent = mødt });
            retur.Add(new ChartValue { ID = 4, Navn = "Mødt aktiv", Procent = mødtudenInaktiv });
            cnn.Close();
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
            cnn.Close();
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
            for (int i = 0; i < regtypes.Count; i++)
            {
                retur.Add(
                    new ChartValue 
                    { 
                        ID = regtypes[i].ID, 
                        Navn = regtypes[i].catName + " " + regtypes[i].TypeName, 
                        Procent = counts[i] 
                    });
            }
            cnn.Close();
            return retur;
        }


        public static List<regType> getAllTypes()
        {
            List<regType> retur = new List<regType>();
            SqlConnection cnn = new SqlConnection(conString);

            cnn.Open();

            SqlCommand cmd;
            String sql = "SELECT * FROM RegistrationTypes r, Categories c where CatID = c.ID";

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
                            CatID = int.Parse(oReader["CatID"].ToString()),
                            catName = oReader["CategoryName"].ToString()

                        });
                }

            }
            cnn.Close();
            return retur;
        }

        public static int getTypeID(String Name)
        {
            int retur = -1;
            SqlConnection cnn = new SqlConnection(conString);

            cnn.Open();

            SqlCommand cmd;
            String sql = "SELECT * FROM RegistrationTypes where TypeName = @Name";

            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("Name", Name);
            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    retur = int.Parse(oReader["ID"].ToString());


                }

            }
            cnn.Close();
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
            cnn.Close();
            return retur;
        }

        public static List<string> GetIncompleteRegistrationsDescriptions(DateTime _date)
        {
            List<string> retur = new List<string>();
            try
            {
                int id = getTypeID("Ikke set");
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "select * from Presents p, students s where p.studentID = s.ID And (Model1 in (0, @id) OR Model2 in (0, @id) OR Model3 in (0, @id) OR Model4 in (0, @id) ) AND Date < @Date";

                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("Date", _date);
                cmd.Parameters.AddWithValue("id", id);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        string s = "";
                        s += oReader["Date"].ToString() + " ";
                        s += oReader["Name"].ToString() + " ";
                        s += oReader["Team"].ToString() + " ";
                        s += " m1 = " + oReader["Model1"].ToString();
                        s += " m2 = " + oReader["Model2"].ToString();
                        s += " m3 = " + oReader["Model3"].ToString();
                        s += " m4 = " + oReader["Model4"].ToString();
                        retur.Add(s);
                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
            return retur;
        }

        public static List<Present> GetIncompleteRegistrations(DateTime beforeDate)
        {
            List<Present> retur = new List<Present>();
            try
            {
                int id = getTypeID("Ikke set");
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();

                SqlCommand cmd;
                string sql = "select * from Presents p, students s where p.studentID = s.ID And (Model1 in (0, @id) OR Model2 in (0, @id) OR Model3 in (0, @id) OR Model4 in (0, @id) ) AND Date < @Date";

                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("Date", beforeDate);
                cmd.Parameters.AddWithValue("id", id);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Present present = Present.MapFromReader(oReader);
                        Student student = Student.MapFromReader(oReader);
                        present.StudentModel = student;
                        retur.Add(present);
                    }
                }
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
            return retur;
        }

        public static List<Student> GetStudents()
        {
            List<Student> retur = new List<Student>();
            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "SELECT * FROM Students where Aktiv = 'true' Order by Name";

                cmd = new SqlCommand(sql, cnn);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        retur.Add(
                            new Student
                            {
                                ID = int.Parse(oReader["ID"].ToString()),
                                Name = oReader["Name"].ToString(),
                                Team = oReader["Team"].ToString(),
                                Aktiv = (bool)oReader["Aktiv"]

                            });
                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                FileHandler.Error(ex);
            }
            return retur;
        }

        public static List<Appointment> GetAppointments()
        {
            List<Appointment> retur = new List<Appointment>();
            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "SELECT * FROM Appointments where StudentId IN (select ID from Students where Aktiv = 1)";

                cmd = new SqlCommand(sql, cnn);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        retur.Add(
                            new Appointment
                            {
                                ID = int.Parse(oReader["ID"].ToString()),
                                StudentID = int.Parse(oReader["StudentID"].ToString()),
                                FromeDate = DateTime.Parse(oReader["FromeDate"].ToString()),
                                ToDate = DateTime.Parse(oReader["ToDate"].ToString()),
                                Day = oReader["Day"].ToString(),
                                Modules = oReader["Modules"].ToString(),
                                Info = oReader["Info"].ToString(),
                                RegistrationTypeID = int.Parse(oReader["RegistrationTypeID"].ToString())

                            });
                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                FileHandler.Error(ex);
            }
            return retur;
        }
        public static void addAppointment(Appointment a)
        {

            try
            {
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();
                String sql = "Insert into Appointments values(@StudentID,@FromeDate,@ToDate,@Day,@Modules,@Info,@RegistrationTypeID)";
                SqlCommand cmd = new SqlCommand(sql, cnn);
                DateTime today = Util.getDateTime();
                cmd.Parameters.AddWithValue("StudentID", a.StudentID);
                cmd.Parameters.AddWithValue("FromeDate", a.FromeDate);
                cmd.Parameters.AddWithValue("ToDate", a.ToDate);
                cmd.Parameters.AddWithValue("Day", a.Day);
                cmd.Parameters.AddWithValue("Modules", a.Modules);
                cmd.Parameters.AddWithValue("Info", a.Info);
                cmd.Parameters.AddWithValue("RegistrationTypeID", a.RegistrationTypeID);
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
        }

        public static List<Category> GetCategory()
        {
            List<Category> retur = new List<Category>();
            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "SELECT * FROM Categories";

                cmd = new SqlCommand(sql, cnn);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        retur.Add(
                            new Category
                            {
                                ID = int.Parse(oReader["ID"].ToString()),
                                CategoryName = oReader["CategoryName"].ToString()

                            });
                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }

            return retur;
        }
        public static void addCategory(Category c)
        {

            try
            {
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();
                String sql = "Insert into Categories values(@CategoryName)";
                SqlCommand cmd = new SqlCommand(sql, cnn);
                DateTime today = Util.getDateTime();
                cmd.Parameters.AddWithValue("CategoryName", c.CategoryName);
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
        }


        public static List<AbsentType> GetAbcentTypes()
        {
            List<AbsentType> retur = new List<AbsentType>();

            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "SELECT t.ID, TypeName,categoryName FROM Categories c, Registrationtypes t where c.id = t.catID Order by c.id desc";

                cmd = new SqlCommand(sql, cnn);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        retur.Add(
                            new AbsentType
                            {
                                ID = int.Parse(oReader["ID"].ToString()),
                                TypeName = oReader["TypeName"].ToString(),
                                CatName = oReader["categoryName"].ToString()

                            });
                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }


            return retur;

        }

        public static Present GetPresent(DateTime date, int studentID)
        {
            Present p = null;

            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                string todayParam = "@today";
                string studentIdParam = "@studentId";
                string sql = $"SELECT * FROM {Present.TABLE_NAME} WHERE {Present.DATE} = {todayParam} AND {Present.STUDENT_ID} = {studentIdParam}";

                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue(studentIdParam, studentID);
                cmd.Parameters.AddWithValue(todayParam, date);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        p = new Present()
                        {
                            Date = DateTime.Parse(oReader["Date"].ToString()),
                            Model1 = int.Parse(oReader[Present.MODULE_1].ToString()),
                            Model2 = int.Parse(oReader[Present.MODULE_2].ToString()),
                            Model3 = int.Parse(oReader[Present.MODULE_3].ToString()),
                            Model4 = int.Parse(oReader[Present.MODULE_4].ToString()),
                            StudentID = int.Parse(oReader[Present.STUDENT_ID].ToString())
                        };
                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }


            return p;
        }

        private static List<Present> getPresents()
        {
            List<Present> retur = new List<Present>();
            SqlConnection cnn = new SqlConnection(conString);

            cnn.Open();

            SqlCommand cmd;
            String sql = "SELECT * FROM Presents";

            cmd = new SqlCommand(sql, cnn);
            using (SqlDataReader oReader = cmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    retur.Add(
                        new Present
                        {
                            Date = DateTime.Parse(oReader["Date"].ToString()),
                            Model1 = int.Parse(oReader["Model1"].ToString()),
                            Model2 = int.Parse(oReader["Model2"].ToString()),
                            Model3 = int.Parse(oReader["Model3"].ToString()),
                            Model4 = int.Parse(oReader["Model4"].ToString()),
                            StudentID = int.Parse(oReader["StudentID"].ToString())

                        });
                }

            }
            cnn.Close();
            return retur;
        }

        public static List<Student> getNotPresent(DateTime today)
        {

            List<Student> retur = new List<Student>();

            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "Select * from Students s, Presents p where s.ID = p.StudentID And s.Aktiv = 1 AND Date >= @Date AND(Model1 = 0 Or Model1 in(Select ID From RegistrationTypes Where TypeName = 'Ikke set')) order by Name";

                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("date", today);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        retur.Add(new Student()
                        {
                            ID = int.Parse(oReader["ID"].ToString()),
                            Name = oReader["Name"].ToString(),
                            Team = oReader["Team"].ToString(),
                            Aktiv = (bool)oReader["Aktiv"]
                        });

                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                FileHandler.Error(ex);
            }

            return retur;
        }

        public static List<Student> getNotPresentToday(DateTime today)
        {

            List<Student> retur = new List<Student>();

            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "Select * from Students s, Presents p where s.ID = p.StudentID And s.Aktiv = 1 AND Date = @Date AND(Model1 = 0 Or Model1 in(Select ID From RegistrationTypes Where TypeName = 'Ikke set')) order by Name";

                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("date", today);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        retur.Add(new Student()
                        {
                            ID = int.Parse(oReader["ID"].ToString()),
                            Name = oReader["Name"].ToString(),
                            Team = oReader["Team"].ToString(),
                            Aktiv = (bool)oReader["Aktiv"]
                        });

                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                FileHandler.Error(ex);
            }

            return retur;
        }

        public static void AddPresent(Present p)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();
                    string dateParam = "@Date";
                    string modul1Param = "@Modul1";
                    string modul2Param = "@Modul2";
                    string modul3Param = "@Modul3";
                    string modul4Param = "@Modul4";
                    string studentIdParam = "@StudentId";
                    string query = $"INSERT INTO {Present.TABLE_NAME} VALUES({dateParam},{studentIdParam},{modul1Param},{modul2Param},{modul3Param},{modul4Param})";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(dateParam, p.Date);
                        command.Parameters.AddWithValue(studentIdParam, p.StudentID);
                        command.Parameters.AddWithValue(modul1Param, p.Model1);
                        command.Parameters.AddWithValue(modul2Param, p.Model2);
                        command.Parameters.AddWithValue(modul3Param, p.Model3);
                        command.Parameters.AddWithValue(modul4Param, p.Model4);
                        int res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                FileHandler.Error(ex);
            }

        }

        public static void UpdatePresent(Present p)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "update Presents Set Model1 = @Model1, Model2 = @Model2, Model3 = @Model3, Model4 = @Model4 Where StudentID = @StudentID and Date = @Date";


                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("StudentID", p.StudentID);
                cmd.Parameters.AddWithValue("Date", p.Date);
                cmd.Parameters.AddWithValue("Model1", p.Model1);
                cmd.Parameters.AddWithValue("Model2", p.Model2);
                cmd.Parameters.AddWithValue("Model3", p.Model3);
                cmd.Parameters.AddWithValue("Model4", p.Model4);

                int res = cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {
                FileHandler.Error(ex);
            }
        }

        public static void AddType(RegistrationType t)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "Insert Into RegistrationTypes Values(@Name,@CatID)";


                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("Name", t.TypeName);
                cmd.Parameters.AddWithValue("CatID", t.CatID);

                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
        }

        public static User GetUser(User us)
        {
            User retur = null;


            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "SELECT * from Users where Usernane = @Usernane and @Password = Password";

                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("Usernane", us.Usernane);
                cmd.Parameters.AddWithValue("Password", us.Password);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        retur = new User()
                        {

                            Usernane = oReader["Usernane"].ToString(),
                            UserRole = int.Parse(oReader["UserRole"].ToString()),
                            Password = oReader["Password"].ToString(),
                        };
                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
            return retur;
        }
        public static void RemoveHoliday(Holiday h)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "Delete from Holidays where ID = @ID";

                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("ID", h.ID);
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
        }

        public static List<Holiday> GetHolidays()
        {
            List<Holiday> retur = new List<Holiday>();

            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "SELECT * from Holidays";

                cmd = new SqlCommand(sql, cnn);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        retur.Add(new Holiday()
                        {

                            From = DateTime.Parse(oReader["From"].ToString()),
                            Too = DateTime.Parse(oReader["Too"].ToString()),
                            ID = int.Parse(oReader["ID"].ToString())
                        });
                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
            return retur;
        }


        public static int AddUserRole(UserRole ur)
        {
            int ID = -1;
            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "Insert into UserRoles Values(@Name); SELECT SCOPE_IDENTITY()";

                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("Name", ur.Name);
                ID = int.Parse(cmd.ExecuteScalar().ToString());
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }
            return ID;
        }

        public static List<Student> GetHold()
        {
            List<Student> liste = new List<Student>();

            try
            {
                SqlConnection cnn = new SqlConnection(conString);

                cnn.Open();

                SqlCommand cmd;
                String sql = "SELECT Distinct(Team) from Students";

                cmd = new SqlCommand(sql, cnn);
                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        liste.Add(new Student() { Team = oReader["Team"].ToString() });
                    }

                }
                cnn.Close();
            }
            catch (Exception ex)
            {

                FileHandler.Error(ex);
            }

            return liste;
        }

        public static bool notHoliday()
        {

            bool retur = true;
            foreach (Holiday h in SQLDB.GetHolidays())
            {
                if (DateTime.Now >= h.From && DateTime.Now <= h.Too)
                {
                    retur = false;
                    break;
                }
            }
            return retur;
        }


        public static List<Student> GetStundentsOnTeam(string Team)
        {
            List<Student> liste = new List<Student>();

            var quarry = from s in SQLDB.GetStudents()
                         where s.Team == Team && s.Aktiv == true
                         select s;

            liste = quarry.ToList();

            return liste;
        }

        public static int GetAftaleFri(int id)
        {
            int i = -1;
            var quary = from ty in SQLDB.GetAbcentTypes()
                        where ty.ID.Equals(id)
                        select ty;


            for (int ind = 0; ind < SQLDB.GetAbcentTypes().Count; ind++)
            {
                if (SQLDB.GetAbcentTypes()[ind].ID == (quary.FirstOrDefault()).ID)
                {
                    i = ind;
                    break;
                }

            }


            return i;
        }

        public static List<AppointmentStudent> getAllPresents(int studentID)
        {
            List<AppointmentStudent> retur = new List<AppointmentStudent>();
            var quary = (from a in SQLDB.GetAppointments()
                         join s in SQLDB.GetStudents() on a.StudentID equals s.ID
                         select new AppointmentStudent
                         {
                             ID = a.ID,
                             StudentID = s.ID,
                             Name = s.Name,
                             Team = s.Team,
                             FromeDate = a.FromeDate,
                             ToDate = a.ToDate,
                             Day = a.Day,
                             Modules = a.Modules,
                             Info = a.Info
                         })
                               .ToList();

            if (studentID != -1)
            {
                quary = quary.Where(aps => aps.StudentID == studentID).ToList();
            }
            retur = quary;

            return retur;
        }
    }

}
