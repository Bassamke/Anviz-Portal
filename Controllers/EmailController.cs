using AnvizWeb.Models;
using AnvizWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnvizWeb.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        private readonly AnvizRepository _repository;
        public EmailController(AnvizRepository repository) {
        _repository= repository;
        }
        public async Task<IActionResult> Index()
        {
            var emailSettings=await _repository.getEmailSettings();
            return View(emailSettings);
        }
        public async Task<IActionResult> Edit()
        {
            var emailSettings = await _repository.getEmailSettings();
            return View(emailSettings);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmailSettings emailSettings)
        {
             _repository.UpdateEmailSettings(emailSettings);
            _repository.LogActivity(HttpContext.Session.GetString("userName"), HttpContext.Session.GetString("IPAddress"), $"Edited Email Settings");
            return RedirectToAction("Index", "Email");
        }
    }
}
