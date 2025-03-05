using EmployeeTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTask.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();
        // GET: Employee
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                dbHelper.AddEmployee(employee);
                ViewBag.Message = "Employee added successfully!";
                return RedirectToAction("Create");
            }

            return View(employee);
        }
    }
}