using SecurityClaim = System.Security.Claims.Claim;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;
using System.Security.Claims;

namespace CMCS_Application.Controllers
{
    public class LoginController : Controller
    {
        private static List<Lecturer> Lecturers = new List<Lecturer>(); 

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string email, string role)
        {
            var UserRole = new List<string> { "Lecturer", "Program Coordinator", "Academic Manager", "HR" };

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || !UserRole.Contains(role))
            {
                ViewBag.Error = "Please enter a username, email, and select your role.";
                return View();
            }

            // Create user claims
            // Create user claims using the aliased SecurityClaim
            var claims = new List<SecurityClaim>
            {
              new SecurityClaim(ClaimTypes.Name, username),
              new SecurityClaim(ClaimTypes.Email, email),
              new SecurityClaim(ClaimTypes.Role, role)
            };


            // Create identity
            var identity = new ClaimsIdentity(claims, "CookieAuth");

            // Sign in the user
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("CookieAuth", principal);

            Lecturers.Add(new Lecturer
            {
                LecturerName = username,
                LecturerEmail = email
            });

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");

            return RedirectToAction("Index", "Home");
        }
    }
}
