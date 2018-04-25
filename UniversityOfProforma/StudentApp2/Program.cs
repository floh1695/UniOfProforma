using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApp2
{
    class Program
    {
        public const string CONNECTION_STRING = @"Server=localhost\SQLEXPRESS;Database=University;Trusted_Connection=True;";

        static int FullNameToStudentID(string fullname)
        {
            int studentID;
            using (var connection = new SqlConnection(CONNECTION_STRING))
            using (var command = new SqlCommand("select top(1) ID from Students where [FullName] = @name", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@name", fullname);
                var reader = command.ExecuteReader();
                reader.Read();
                int.TryParse(reader["ID"].ToString(), out studentID);
            }
            return studentID;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("welcome to the student application");
            Console.WriteLine("1: enroll in course");
            Console.WriteLine("2: check where enrolled");
            int.TryParse(Console.ReadLine(), out int mainChoice);
            var options = new Dictionary<int, Action>
            {
                {
                    1,
                    () => 
                    {
                        using (var connection = new SqlConnection(CONNECTION_STRING))
                        using (var command = new SqlCommand("select [ID], [Name] from Courses", connection))
                        {
                            connection.Open();
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["ID"]}: {reader["Name"]}");
                            }
                        }
                        Console.Write("which course? ");
                        int.TryParse(Console.ReadLine(), out int courseChoice);

                        Console.Write("what is your name? ");
                        var studentID = FullNameToStudentID(Console.ReadLine());

                        using (var connection = new SqlConnection(CONNECTION_STRING))
                        using (var command = new SqlCommand("insert into Enrollment (StudentID, CourseID) values (@student, @course)", connection))
                        {
                            connection.Open();
                            command.Parameters.AddWithValue("@student", studentID);
                            command.Parameters.AddWithValue("@course", courseChoice);
                            command.ExecuteNonQuery();
                        }
                    }
                },
                {
                    2,
                    () =>
                    {
                        Console.Write("what is your name? ");
                        var studentID = FullNameToStudentID(Console.ReadLine());

                        //
                        using (var connection = new SqlConnection(CONNECTION_STRING))
                        using (var command = new SqlCommand("select [Name] from Enrollment join Students on StudentID = Students.ID join Courses on CourseID = Courses.ID where StudentID = @student", connection))
                        {
                            connection.Open();
                            command.Parameters.AddWithValue("@student", studentID);
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                Console.WriteLine(reader["Name"]);
                            }
                        }
                    }
                }
            };
            options[mainChoice]();
        }
    }
}
