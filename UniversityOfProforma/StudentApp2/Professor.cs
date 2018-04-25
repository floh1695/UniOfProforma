using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApp2
{
    class Professor : ISqlInsert
    {
        int ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        void ISqlInsert.Insert()
        {
            using (var connection = new SqlConnection(Program.CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand("insert into [Professors]([Name], [Title]) values (@Name, @Title)", connection))
                {
                    command.Parameters.AddWithValue("@Name", this.Name);
                    command.Parameters.AddWithValue("@Title", this.Title);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
