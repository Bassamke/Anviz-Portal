using AnvizWeb.Models;
using AnvizWeb.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

namespace AnvizWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly  AnvizRepository _repository;
        public AccountController(AnvizRepository repository) { 
        _repository= repository;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login loginDetails)
        {
            loginDetails.Password = await HashPassword(loginDetails.Password);
            var Response =await _repository.SignInUser(loginDetails);
            if (Response== "SUCCESS")
            {

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginDetails.Email)

                    };

                //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
                var principal = new ClaimsPrincipal(identity);
                //SignInAsync is a Extension method for Sign in a principal for the specified scheme.
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    principal, new AuthenticationProperties() { IsPersistent = true });

                HttpContext.Session.SetString("userName", loginDetails.Email);
                HttpContext.Session.SetString("IPAddress", HttpContext.Connection.RemoteIpAddress.ToString());
                return RedirectToAction("Index", "Home");
            }
            else if (Response== "FIRSTLOGIN")
            {
                return RedirectToAction("ChangePassword");
            }
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword user)
        {
            user.OldPassword = await HashPassword(user.OldPassword);
            user.Password = await HashPassword(user.Password);

            _repository.UpdateUserPassword(user);
            return RedirectToAction("Login");
        }
        [Authorize]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Login loginDetails)
        {
            string plainPassword = await generatePassword();
            loginDetails.Password= await HashPassword(new string(plainPassword));
            _repository.AddUser(loginDetails);
            await sendEmail("ANVIZ USER REGISTRATION ALERT", $"Dear {loginDetails.Email} you have been added as a user in the Anviz Portal. Use the password {plainPassword} to Login. You will be required to change your password on first login", loginDetails.Email);
            _repository.LogActivity(HttpContext.Session.GetString("userName"), HttpContext.Session.GetString("IPAddress"), $"Added new user {loginDetails.Email}");
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            //SignOutAsync is Extension method for SignOut
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page
            return RedirectToAction("Login");
        }
        public async Task<bool> sendEmail(string subject, string emailMmessage, string receiver)
        {
            try
            {
                MailMessage message = new MailMessage();
              

               EmailSettings emailSettings= await _repository.getEmailSettings();

                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(emailSettings.Emailaccount, "ANVIZ REGISTRATION ALERT");
                message.To.Add(receiver);
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = emailMmessage;
                smtp.UseDefaultCredentials = false;
                smtp.Port = emailSettings.Port;
                smtp.Host = emailSettings.Smtp; //for gmail host;
                smtp.EnableSsl = emailSettings.UseSSL;
                smtp.Credentials = new NetworkCredential(emailSettings.Emailaccount, emailSettings.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                string userState = "test message1";

                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed");
            }
            return true;
        }
        public async Task<string> generatePassword()
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";
            const int passwordLength = 6;

            var randomBytes = new byte[passwordLength];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            var password = new char[passwordLength];
            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = allowedChars[randomBytes[i] % allowedChars.Length];
            }
            return (new string(password));
        }
        public async Task<string> HashPassword(string password)
        {
            string saltString = "anvizthings";
            byte[] salt = Encoding.UTF8.GetBytes(saltString);

            using var sha256 = SHA256.Create();

            // Combine password and salt
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordAndSaltBytes = new byte[passwordBytes.Length + salt.Length];
            Array.Copy(passwordBytes, passwordAndSaltBytes, passwordBytes.Length);
            Array.Copy(salt, 0, passwordAndSaltBytes, passwordBytes.Length, salt.Length);

            // Hash the password and salt
            byte[] hash = sha256.ComputeHash(passwordAndSaltBytes);
            return Convert.ToBase64String(hash);
        }
    }
}
