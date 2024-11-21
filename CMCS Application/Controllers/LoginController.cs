using Microsoft.AspNetCore.Mvc;

namespace CMCS_Application.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        } 

        [HttpPost]
        public IActionResult Login(string username, string role)
        {
            var UserRole = new List<string> { "Lecturer", "Program Coordinator", "Academic Manager", "HR" };

            if(string.IsNullOrEmpty(username) || !UserRole.Contains(role))
            {
                ViewBag.Error = "Please enter a username and select your role.";
                return View();
            }

            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetString("Role", role);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
