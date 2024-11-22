using SecurityClaim = System.Security.Claims.Claim; //Alias to avoid complexity
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;
using System.Security.Claims;

namespace CMCS_Application.Controllers
{
    public class LoginController : Controller
    {
        //store lecturer data as soon as lecturer logs in
        //https://stackoverflow.com/questions/17219471/saving-a-list-of-entities-to-the-db-mvc
        private static List<Lecturer> Lecturers = new List<Lecturer>(); 

        [HttpGet]
        public IActionResult Login()
        {
            return View();//return index after logging in
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string email, string role)
        {
            //different user roles in the application
            var UserRole = new List<string> { "Lecturer", "Program Coordinator", "Academic Manager", "HR" };

            //if a field is empty an error will be displayed
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || !UserRole.Contains(role))
            {
                ViewBag.Error = "Please enter a username, email, and select your role.";
                return View();
            }

            //create user claims using the aliased SecurityClaim
            //https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claim?view=net-8.0
            //https://stackoverflow.com/questions/18781404/custom-claim-using-complex-type-is-not-deserialized-correctly
            var claims = new List<SecurityClaim>
            {
              new SecurityClaim(ClaimTypes.Name, username), 
              new SecurityClaim(ClaimTypes.Email, email),
              new SecurityClaim(ClaimTypes.Role, role)
            };

            //https://stackoverflow.com/questions/17769011/how-does-cookie-based-authentication-work
            //create identity for the authenticated user
            var identity = new ClaimsIdentity(claims, "CookieAuth");

            //sign in the user
            //https://learn.microsoft.com/en-us/dotnet/api/system.security.claims?view=net-9.0
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("CookieAuth", principal);

            if (role == "Lecturer")
            {
                //create lecturer object to store the lecturer data
                var lecturer = new Lecturer
                {
                    Id = ClaimMemory.IncrementLecturerId(), //assign a unique id
                    LecturerName = username, //store username
                    LecturerEmail = email, //store email
                    Role = role //store the role
                };

                ClaimMemory.Lecturers.Add(lecturer); //add to the list
            }
            //redirect to home after logging in
            return RedirectToAction("Index", "Home");
        }

        //logout function
        public async Task<IActionResult> Logout()
        {
            //sign the current user out
            await HttpContext.SignOutAsync("CookieAuth");

            //clear cookies on signout
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            //redirect to home page after signout
            return RedirectToAction("Index", "Home");
        }
    }
}
