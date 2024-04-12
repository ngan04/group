using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Models;
using System.Text.Json;

namespace SIMS_Demo.Controllers
{
    public class StudentController : Controller
    {
        static List<Student>? students = new List<Student>();

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            //read Teachers from a file
          // students = ReadFileToStudentList("data.json");
            //search and delete
            var result = students.FirstOrDefault(t => t.Id == Id);
            if (result != null)
            {
                students.Remove(result);
                //update json file
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(students, options);
                using (StreamWriter writer = new StreamWriter("data.json"))
                {
                    writer.Write(jsonString);
                }
            }
            return RedirectToAction("Index");


        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetString("Role");
             //students = ReadFileToStudentList("data.json");
            return View(students);
        }

        public static List<Student>? ReadFileToTeacherList(String filePath)
        {
            // Read a file
            string readText = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Student>>(readText);
        }

        //click Hyperlink
        [HttpGet]
        public IActionResult NewStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewStudent(Student student)
        {
            students.Add(student);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(students, options);
            using (StreamWriter writer = new StreamWriter("data.json"))
            {
                writer.Write(jsonString);
            }
            return Content(jsonString);
        }
    }
}
