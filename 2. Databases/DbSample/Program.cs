using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\s_ber\source\repos\DbSample\Library.mdf;Integrated Security=True"))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Authors", conn);
                var reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Console.WriteLine($"{reader["FistName"]}, {reader["LastName"]}");
                }
                conn.Close();
            }
            
        }
    }
}
