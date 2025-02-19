using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using ElectroLingua.Models;

namespace ElectroLingua
{
    public class DatabaseHelper
    {
        private const string DbFileName = "ElectroLingua.db";
        private static readonly string DbPath = Path.Combine(Environment.CurrentDirectory, DbFileName);

        private static readonly string ConnectionString = $"Data Source={DbPath};Version=3;";

        public DatabaseHelper()
        {
            if (!File.Exists(DbPath))
            {
                CreateDatabase();
                CreateTables();
            }
        }


        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DbPath);
        }

        private void CreateTables()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Create Students table
                string createStudentsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Students (
                        StudentID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        Email TEXT,
                        Phone TEXT
                    );";
                using (var command = new SQLiteCommand(createStudentsTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create Courses table
                string createCoursesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Courses (
                        CourseID INTEGER PRIMARY KEY AUTOINCREMENT,
                        CourseName TEXT NOT NULL,
                        Description TEXT,
                        Price REAL
                    );";
                using (var command = new SQLiteCommand(createCoursesTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create Teachers table
                string createTeachersTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Teachers (
                        TeacherID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        Email TEXT
                    );";
                using (var command = new SQLiteCommand(createTeachersTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }


                //Create Enrollments table
                string createEnrollmentsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Enrollments (
                        EnrollmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentID INTEGER NOT NULL,
                        CourseID INTEGER NOT NULL,
                        EnrollmentDate DATETIME,
                        FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
                        FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
                    );";
                using (var command = new SQLiteCommand(createEnrollmentsTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }


                connection.Close();
            }
        }

        //Пример метода для получения всех студентов
        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Students;";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student
                            {
                                StudentID = Convert.ToInt32(reader["StudentID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString()
                            };
                            students.Add(student);
                        }
                    }
                }
            }

            return students;
        }

        //Пример метода для добавления нового студента
        public void AddNewStudent(Student student)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string insertQuery = @"
                    INSERT INTO Students (FirstName, LastName, Email, Phone)
                    VALUES (@FirstName, @LastName, @Email, @Phone);";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@Email", student.Email);
                    command.Parameters.AddWithValue("@Phone", student.Phone);

                    command.ExecuteNonQuery();
                }
            }
        }

        //Добавь другие методы CRUD здесь (Create, Read, Update, Delete)
    }
}