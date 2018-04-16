using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityOfProforma
{
    class Program
    {
        public const string CONNECTION_STRING = @"Server=localhost\SQLEXPRESS;Database=University;Trusted_Connection=True;";

        static string WriteRead(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Which action would you like to use?");
            Console.WriteLine("1: Add Professor");
            Console.WriteLine("2: Add Course");
            Console.WriteLine("3: View all enrolled students");
            Console.WriteLine("4: Dump all data");
            int.TryParse(Console.ReadLine(), out int mainChoice);
            var mainChoices = new Dictionary<int, Action>
            {
                {
                    1,
                    () =>
                    {
                        var name = WriteRead("What is this professors name?");
                        var title = WriteRead("What is this professors title?");

                        var newProfessor = new Professor
                        {
                            Name = name,
                            Title = title
                        };
                        (newProfessor as ISqlInsert).Insert();
                    }
                },
                {
                    2,
                    () =>
                    {
                        var number = WriteRead("Course number?");
                        int.TryParse(WriteRead("Course level?"), out int level);
                        var name = WriteRead("Course name?");
                        var room = WriteRead("Course room?");
                        var startTime = WriteRead("Course start time? f => HH:MM[:SS]");

                        var newCourse = new Course
                        {
                            Number = number,
                            Level = level,
                            Name = name,
                            Room = room,
                            StartTime = startTime
                        };
                        (newCourse as ISqlInsert).Insert();
                    }
                },
                {
                    3,
                    () =>
                    {
                        using (var connection = new SqlConnection(CONNECTION_STRING))
                        {
                            connection.Open();
                            using (var command = new SqlCommand("select * from [Enrollment] join [Students] on [Enrollment].StudentID = [Students].ID", connection))
                            {
                                var reader = command.ExecuteReader();
                                while (reader.Read()) {
                                    Console.WriteLine(reader["FullName"]);
                                }
                            }
                            connection.Close();
                        }
                    }
                },
                {
                    4,
                    () =>
                    {
                        var courseNameList = new List<Course>();

                        using (var connection = new SqlConnection(CONNECTION_STRING))
                        {
                            connection.Open();
                            using (var command = new SqlCommand("select [ID], [Name] from [Courses]", connection))
                            {
                                var reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    courseNameList.Add(new Course
                                    {
                                        ID = int.Parse(reader["ID"].ToString()),
                                        Name = reader["Name"].ToString()
                                    });
                                }
                            }
                            connection.Close();
                        }

                        courseNameList.ForEach(course =>
                        {
                            Console.WriteLine($"Course: {course.Name}");
                            using (var connection = new SqlConnection(CONNECTION_STRING))
                            {
                                connection.Open();
                                using (var command = new SqlCommand("select top(1) [Title], [Name] from [Jobs] join [Professors] on ProfessorID = Professors.ID where [Jobs].ID = @JobsID", connection))
                                {
                                    command.Parameters.AddWithValue("@JobsID", course.ID);
                                    var reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"Professor: {reader["Title"]}. {reader["Name"]}");
                                    }
                                }
                                connection.Close();
                            }
                            using (var connection = new SqlConnection(CONNECTION_STRING))
                            {
                                connection.Open();
                                Console.WriteLine("Enrolled:");
                                using (var command = new SqlCommand("select [FullName] from [Enrollment] join [Students] on StudentID = Students.ID where [Enrollment].CourseID = @CourseID", connection))
                                {
                                    command.Parameters.AddWithValue("@CourseID", course.ID);
                                    var reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"\tStudent: {reader["FullName"]}");
                                    }
                                }
                                connection.Close();
                            }
                        });

                    }
                }
            };
            mainChoices[mainChoice]();

            Console.ReadLine();
        }
    }
}
