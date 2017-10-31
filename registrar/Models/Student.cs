using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Registrar.Models
{
  public class Student
  {
    private string _description;
    private int _id;
    private string _enrollmentDate;

    public Student(string description, string enrollmentDate, int id = 0)
    {
      _description = description;
      _id = id;
      _enrollmentDate = enrollmentDate;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool descriptionEquality = this.GetDescription() == newStudent.GetDescription();
        bool enrollmentdateEquality = this.GetEnrollmentDate() == newStudent.GetEnrollmentDate();
        return (idEquality && descriptionEquality && enrollmentdateEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetDescription().GetHashCode();
    }

    public string GetDescription()
    {
      return _description;
    }
    public int GetId()
    {
      return _id;
    }

    public string GetEnrollmentDate()
    {
      return _enrollmentDate;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students (description, enrollmentdate) VALUES (@description, @enrollmentDate);";

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@description";
      description.Value = this._description;
      cmd.Parameters.Add(description);

      MySqlParameter enrollmentdate = new MySqlParameter();
      enrollmentdate.ParameterName = "@enrollmentDate";
      enrollmentdate.Value = this._enrollmentDate;
      cmd.Parameters.Add(enrollmentdate);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students ORDER BY enrollmentdate;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentDescription = rdr.GetString(1);
        string studentEnrollmentDate = rdr.GetString(2);
        Student newStudent = new Student(studentDescription, studentEnrollmentDate, studentId);
        allStudents.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }
    public static Student Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int studentId = 0;
      string studentName = "";
      string studentEnrollmentDate = "";

      while(rdr.Read())
      {
        studentId = rdr.GetInt32(0);
        studentName = rdr.GetString(1);
        studentEnrollmentDate = rdr.GetString(2);
      }
      Student newStudent = new Student(studentName, studentEnrollmentDate, studentId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newStudent;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM students;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void DeleteStudents()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        MySqlCommand cmd = new MySqlCommand("DELETE FROM students WHERE id = @StudentId; DELETE FROM courses_students WHERE student_id = @StudentId;", conn);
        MySqlParameter studentIdParameter = new MySqlParameter();
        studentIdParameter.ParameterName = "@StudentId";
        studentIdParameter.Value = this.GetId();

        cmd.Parameters.Add(studentIdParameter);
        cmd.ExecuteNonQuery();

        if (conn != null)
        {
          conn.Close();
        }
      }


      public static List<Student> GetAlphaList()
      {
        List<Student> allStudents = new List<Student> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM students ORDER BY enrollmentdate;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int studentId = rdr.GetInt32(0);
          string studentDescription = rdr.GetString(1);
          string studentEnrollmentDate = rdr.GetString(2);
          Student newStudent = new Student(studentDescription, studentEnrollmentDate, studentId);
          allStudents.Add(newStudent);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return allStudents;
      }

      public void AddCourse(Course newCourse)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

        MySqlParameter course_id = new MySqlParameter();
        course_id.ParameterName = "@CourseId";
        course_id.Value = newCourse.GetId();
        cmd.Parameters.Add(course_id);

        MySqlParameter student_id = new MySqlParameter();
        student_id.ParameterName = "@StudentId";
        student_id.Value = _id;
        cmd.Parameters.Add(student_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn !=null)
        {
          conn.Dispose();
        }
      }

      public List<Course> GetCourses()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT course_id FROM courses_Students WHERE student_id = @studentId;";

        MySqlParameter studentIdParameter = new MySqlParameter();
        studentIdParameter.ParameterName = "@studentId";
        studentIdParameter.Value = _id;
        cmd.Parameters.Add(studentIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<int> courseIds =new List<int> {};
        while(rdr.Read())
        {
          int courseId = rdr.GetInt32(0);
          courseIds.Add(courseId);
        }
        rdr.Dispose();

        List<Course> courses = new List<Course> {};
        foreach (int courseId in courseIds)
        {
          var courseQuery = conn.CreateCommand() as MySqlCommand;
          courseQuery.CommandText = @"SELECT * FROM courses WHERE id = @CourseId;";

          MySqlParameter courseIdParameter = new MySqlParameter();
          courseIdParameter.ParameterName = "@CourseId";
          courseIdParameter.Value = courseId;
          courseQuery.Parameters.Add(courseIdParameter);

          var courseQueryRdr = courseQuery.ExecuteReader() as MySqlDataReader;
          while(courseQueryRdr.Read())
          {
            int thisCourseId = courseQueryRdr.GetInt32(0);
            string courseName = courseQueryRdr.GetString(1);
            Course foundCourse = new Course(courseName, thisCourseId);
            courses.Add(foundCourse);
          }
          courseQueryRdr.Dispose();
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return courses;
      }
    }
  }
