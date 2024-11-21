using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;
using System.Linq;

namespace CMCS_Application.Controllers
{
    public class LecturerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(ClaimMemory.Lecturers);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var lecturer = ClaimMemory.Lecturers.FirstOrDefault(x => x.Id == id);
            if(lecturer == null)
            {
                return NotFound();

            }
            return View(lecturer);
        }

        [HttpPost]
        public IActionResult Edit(Lecturer lecturer)
        {
            var currentInfo = ClaimMemory.Lecturers.FirstOrDefault(x => x.Id == lecturer.Id);
            if(currentInfo != null)
            {
                currentInfo.LecturerName = lecturer.LecturerName;
                currentInfo.LecturerEmail = lecturer.LecturerEmail;
            }
            return RedirectToAction("Index");
        }
    }
}
