using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SIMS_Demo.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpPost]
        public IActionResult Login(User user)
        {
            List<User>? users = ReadFileToUserList("users.json");
            var result =
                users.FirstOrDefault(u => u.Name == user.Name && u.Pass == user.Pass);
            if (result !=null)
            {
                HttpContext.Session.SetString("UserName", result.Name);
                HttpContext.Session.SetString("Role", result.Role);
                return RedirectToAction("Index", "Teacher");
            }
            else
            {
                ViewBag.error = "Invalid user";
                return View();
            }

        }

        public  List<User>? ReadFileToUserList(String filePath)
        {
            // Read a file
            string readText = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(readText);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}

