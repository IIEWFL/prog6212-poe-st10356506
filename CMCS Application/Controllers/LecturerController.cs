using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMCS_Application.Controllers
{
    [Authorize]
    public class LecturerController : Controller
    {
        //only allow fr to access this view
        [HttpGet]
        [Authorize(Roles = "HR")]
        public IActionResult Index()
        {
            //return all the lecturers from ClaimMemory.Lecturers
            return View(ClaimMemory.Lecturers);
        }

        [HttpGet]
        [Authorize(Roles = "HR")]
        //Update lecturer data
        public IActionResult Edit(int id)
        {
            //pull lecturer from ID
            //https://stackoverflow.com/questions/30055651/how-to-update-value-in-a-list-using-linq
            var lecturer = ClaimMemory.Lecturers.FirstOrDefault(x => x.Id == id);

            //if lecturer isnt found
            if(lecturer == null)
            {
                return NotFound();

            }
            //return the lecturer for editing
            return View(lecturer);
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        public IActionResult Edit(Lecturer lecturer)
        {
            //https://stackoverflow.com/questions/30055651/how-to-update-value-in-a-list-using-linq
            var currentInfo = ClaimMemory.Lecturers.FirstOrDefault(x => x.Id == lecturer.Id);

            //if lecturer is found update its details
            if(currentInfo != null)
            {
                //updates name and email
                currentInfo.LecturerName = lecturer.LecturerName;
                currentInfo.LecturerEmail = lecturer.LecturerEmail;
            }
            //redirect to index after updates
            return RedirectToAction("Index");
        }
    }
}
