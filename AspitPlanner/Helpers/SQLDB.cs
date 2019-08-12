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
        private static readonly string conString = ConfigurationManager.ConnectionStrings["DBCon"].ConnectionString;

        
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
    }
}
