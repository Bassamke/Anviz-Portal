using AnvizWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnvizWeb.Controllers
{
    [Authorize]
    public class PunchController : Controller
    {
        private readonly AnvizRepository _repository;
        public PunchController(AnvizRepository repository) { 
        _repository= repository;
        }
        public async Task<IActionResult> getPunches()
        {
            var punches=await _repository.getAllEmployeesPunches();
            return View(punches);
        }
        public async Task<IActionResult> getStaffPunches(string EmployeeNumber)
        {
            var punches = await _repository.getPunchesByEmployee(EmployeeNumber);
            return View(punches);
        }
    }
}
