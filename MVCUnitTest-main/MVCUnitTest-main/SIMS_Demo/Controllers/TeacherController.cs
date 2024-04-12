using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SIMS_Demo.Models;


namespace SIMS_Demo.Controllers
{
    public class TeacherController : Controller
    {
        static List<Teacher>? teachers = new List<Teacher>();

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            //read Teachers from a file
            teachers = ReadFileToTeacherList("data.json");
            //search and delete
            var result = teachers.FirstOrDefault(t => t.Id == Id);
            if (result != null)
            {
                teachers.Remove(result);
                //update json file
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(teachers, options);
                using (StreamWriter writer = new StreamWriter("data.json"))
                {
                    writer.Write(jsonString);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var teacher = teachers.FirstOrDefault(t => t.Id == Id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Edit(Teacher teacher, IFormFile image)
        {
            var existingTeacher = teachers.FirstOrDefault(t => t.Id == teacher.Id);
            if (existingTeacher == null)
            {
                return NotFound();
            }

            existingTeacher.Name = teacher.Name;
            existingTeacher.DoB = teacher.DoB;

            if (image != null && image.Length > 0)
            {
                // Lưu trữ ảnh vào thư mục wwwroot/images
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                // Cập nhật URL của ảnh
                existingTeacher.ImageUrl = "/images/" + uniqueFileName;
            }

            // Cập nhật JSON file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(teachers, options);
            System.IO.File.WriteAllText("data.json", jsonString);

            return RedirectToAction("Index");
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetString("Role");
            teachers = ReadFileToTeacherList("data.json");
            return View(teachers);
        }

        public static List<Teacher>? ReadFileToTeacherList(String filePath)
        {
            // Read a file
            string readText = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Teacher>>(readText);
        }

        //click Hyperlink
        [HttpGet]
        public IActionResult NewTeacher()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewTeacher(Teacher teacher)
        {
            teachers.Add(teacher);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(teachers, options);
            using (StreamWriter writer = new StreamWriter("data.json"))
            {
                writer.Write(jsonString);
            }
            return Content(jsonString);
        }
    }
}
