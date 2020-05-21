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
        public async Task<IActionResult> LoadFile(FileModel model)
        {
            if(model.File != null)
            { 
                Employees = await Reader.ReaderAsList(model.File, _appEnvironment.WebRootPath); 
                return RedirectToAction("ShowEmployee");
            }
            else
            {
                ModelState.AddModelError("", "Uploaded file is empty or null.");
                return View(model);
            }
        }

        [HttpGet]
        public JsonResult Employee()
        {
            if(Employees != null)
            { 
                return Json(Employees);
            }
            return Json("Empty");
        }

        [HttpGet]
        public JsonResult Teams()
        {
            if (Employees != null)
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
            return Json("Empty");

        }


        public IActionResult ShowEmployee()
        {
            return View();
        }

   
    }
}
