using AnvizWeb.Models;
using AnvizWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnvizWeb.Controllers
{
    [Authorize]
    public class AnvizController : Controller
    {
        private readonly AnvizRepository _repository;   
        public AnvizController(AnvizRepository repository)
        {
            _repository = repository;
        }
        public async Task< IActionResult> BiometricDevices()
        {
            var devices= await _repository.GetAllBiometricDevicesAsync();
            return View(devices);
        }
        public async Task<IActionResult> EditDevices(int Id)
        {
            var device = await _repository.GetBiometricDeviceAsync(Id);
            return View(device);
        }
        [HttpPost]
        public async Task<IActionResult> EditDevices(BiometricDevices device)
        {
             _repository.UpdateDevice(device);
            _repository.LogActivity(HttpContext.Session.GetString("userName"), HttpContext.Session.GetString("IPAddress"), $"Edited Biometric Device {device.Location}");
            return RedirectToAction("BiometricDevices", "Anviz");

        }
        public async Task<IActionResult> AddDevice(int Id)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDevice(BiometricDevices device)
        {
            _repository.AddDevice(device);
            _repository.LogActivity(HttpContext.Session.GetString("userName"), HttpContext.Session.GetString("IPAddress"), $"Added Biometric Device {device.Location}");
            return RedirectToAction("BiometricDevices", "Anviz");
        }
        public async Task<IActionResult> DeleteDevice(int Id)
        {
            var device = await _repository.GetBiometricDeviceAsync(Id);
            return View(device);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDevice(BiometricDevices device)
        {
            _repository.DeleteDevice(device);
            _repository.LogActivity(HttpContext.Session.GetString("userName"), HttpContext.Session.GetString("IPAddress"), $"Deleted Biometric Device {device.Location}");
            return RedirectToAction("BiometricDevices", "Anviz");
        }
    }
}
