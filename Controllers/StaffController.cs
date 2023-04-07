using AnvizWeb.Models;
using AnvizWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnvizWeb.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {
        private readonly AnvizRepository _repository;
        public StaffController(AnvizRepository repository)
        {
            _repository= repository;
        }
        public async Task<IActionResult> getStaff()
        {
            var staff =await  _repository.getEmployees();
            return View(staff);
        }
        public async Task<IActionResult> staffDetails(string employeeNumber)
        {
            var staff = await _repository.getEmployeeByNumber(employeeNumber);
            return View(staff);
        }
        public async Task<IActionResult> EditStaffStatus(string employeeNumber)
        {
            var staff =await  _repository.getEmployeeByNumber(employeeNumber);
            return View(staff);
        }
        [HttpPost]
        public IActionResult EditStaffStatus(EmployeeData employee)
        {
            _repository.UpdateEmployeeStatus(employee);
            _repository.LogActivity(HttpContext.Session.GetString("userName"), HttpContext.Session.GetString("IPAddress"), $"Edited Staff Status {employee.EmployeeNumber}");
            return RedirectToAction("getStaff");
        }
    }
}
