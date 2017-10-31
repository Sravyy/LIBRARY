using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using Registrar;

namespace Registrar.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        return View();
      }

      [HttpGet("/Courses")]
      public ActionResult Results2()
      {
        return View("Results",Course.GetAll());
      }

      [HttpPost("/Courses")]
      public ActionResult Results()
      {
        Course newCourse = new Course (Request.Form["inputCourse"]);
        newCourse.Save();
        return View (Course.GetAll());
      }

      [HttpGet("/Courses/{id}")]
      public ActionResult ResultStudent(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Course selectedCourse = Course.Find(id);
        List<Student> courseStudents = selectedCourse.GetStudents();
        model.Add("course", selectedCourse);
        model.Add("students", courseStudents);
        return View(model);
      }

      [HttpPost("/Courses/{id}")]
      public ActionResult ResultStudent2(int id)
      {
        string studentDescription = Request.Form["inputStudent"];
        Student newStudent = new Student(studentDescription,(Request.Form["inputDate"]));
        newStudent.Save();
        Dictionary<string, object> model = new Dictionary<string, object>();
        Course selectedCourse = Course.Find(Int32.Parse(Request.Form["course-id"]));
        selectedCourse.AddStudent(newStudent);
        List<Student> courseStudents = selectedCourse.GetStudents();
        model.Add("students", courseStudents);
        model.Add("course", selectedCourse);
        return View("ResultStudent", model);
      }

      [HttpGet("/Courses/{id}/students/new")]
      public ActionResult StudentForm(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Course selectedCourse = Course.Find(id);
        List<Student> allStudents = selectedCourse.GetStudents();
        model.Add("course", selectedCourse);
        model.Add("students", allStudents);
        return View(model);
      }


      [HttpPost("/StudentList")]
      public ActionResult DeletePage2()
      {
        Student.DeleteAll();
        return View();
      }

      [HttpPost("/Courses/{id}/Delete")]
      public ActionResult DeleteCourse(int id)
      {
        Course foundCourse = Course.Find(id);
        foreach (Student classStudent in foundCourse.GetStudents())
          {
            classStudent.DeleteStudents();
          }
        foundCourse.DeleteCourse();
        return View("DeletePage3");
      }

      [HttpPost("/Courses/Delete")]
      public ActionResult DeletePage()
      {
        Course.DeleteAll();
        return View();
      }

      [HttpGet("/StudentList")]
      public ActionResult AlphaList()
      {
        return View(Student.GetAlphaList());
      }

      [HttpGet("/Students/{id}")]
      public ActionResult StudentCourses(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Student selectedStudent = Student.Find(id);
        List<Course> allCourses = selectedStudent.GetCourses();
        model.Add("student", selectedStudent);
        model.Add("course", allCourses);
        return View(model);
      }

      [HttpGet("/student/{id}/courses/new")]
      public ActionResult coursesForm(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Student selectedStudent = Student.Find(id);
        List<Course> allCourses = selectedStudent.GetCourses();
        model.Add("student", selectedStudent);
        model.Add("course", allCourses);
        return View(model);
      }

      [HttpPost("/Students/{id}")]
      public ActionResult StudentCourses2(int id)
      {

        Course newCourse = new Course(Request.Form["inputCourse"]);
        newCourse.Save();

        Dictionary<string, object> model = new Dictionary<string, object>();
        Student selectedStudent = Student.Find(Int32.Parse(Request.Form["student-id"]));
        selectedStudent.AddCourse(newCourse);
        List<Course> allCourses = selectedStudent.GetCourses();
        model.Add("student", selectedStudent);
        model.Add("course", allCourses);
        return View("StudentCourses", model);
      }
    }
  }
