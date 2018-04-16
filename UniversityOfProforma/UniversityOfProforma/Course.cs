using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityOfProforma
{
    class Course : ISqlInsert
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public string Room { get; set; }
        public string StartTime { get; set; }

        void ISqlInsert.Insert()
        {
            using (var connection = new SqlConnection(Program.CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand("insert into [Courses]([Number], [Level], [Name], [Room], [StartTime]) values (@Number, @Level, @Name, @Room, @StartTime)", connection))
                {
                    command.Parameters.AddWithValue("@Number", this.Number);
                    command.Parameters.AddWithValue("@Level", this.Level);
                    command.Parameters.AddWithValue("@Name", this.Name);
                    command.Parameters.AddWithValue("@Room", this.Name);
                    command.Parameters.AddWithValue("@StartTime", this.StartTime);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
