using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Employee_Test_.Models;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using Employee_Test_.Services;
using Microsoft.AspNetCore.Hosting;

namespace Employee_Test_.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _appEnvironment;
        private static List<Employee> Employees;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _logger = logger;
        }

        public IActionResult LoadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadFile(IFormFile uploadedFile)
        {
            if(uploadedFile != null)
            { 
                Employees = await Reader.ReaderAsList(uploadedFile, _appEnvironment.WebRootPath);
            }
            return RedirectToAction("ShowEmployee");
        }

        [HttpGet]
        public JsonResult Employee()
        {
            return Json(Employees);
        }

        [HttpGet]
        public JsonResult Teams()
        {
            List<string> teams = new List<string>();
            foreach(var i in Employees)
            {
                if(teams.FirstOrDefault(t => t == i.TeamName) == null)
                {
                    teams.Add(i.TeamName);
                }
            }
            return Json(teams);
        }


        public IActionResult ShowEmployee()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
