using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;

namespace CMCS_Application.Controllers
{
    [Authorize]
    public class ClaimController : Controller
    {
        //validate submitted data
        private readonly IValidator<Claim> _validator; //https://docs.fluentvalidation.net/en/latest/aspnet.html
        //increment unique id by 1 for each claim
        private static int _claimUniqueID = 1; //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members?form=MG0AV3#static-fields
        

        public ClaimController(IValidator<Claim> validator)
        {
            _validator = validator;
        }

        //https://stackoverflow.com/questions/61543553/how-to-add-a-button-in-a-asp-net-mvc-and-link-a-click-event-from-c-sharp-code/61543699#61543699
        [HttpGet]
        //define which users can access this view
        [Authorize(Roles = "Lecturer, Academic Manager")]
        public IActionResult ClaimForm()
        {
            return View();
        }

        //https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult?view=aspnetcore-8.0&form=MG0AV3

        [HttpPost]
        [Authorize(Roles = "Lecturer")]
        public IActionResult ClaimForm(Claim claim, List<IFormFile> Documents)
        {
            claim.Documents = Documents;
            claim.ClaimId = _claimUniqueID++;

            //handle file uploads only if files are provided
            if (Documents != null && Documents.Count > 0)
            {
                claim.Documents = Documents;
            }
            else
            {
                claim.Documents = new List<IFormFile>();  //if no files are uploaded it will be null in the list
            }

            ValidationResult result = _validator.Validate(claim); //validate the data inputted

            if (!result.IsValid)
            {
                foreach(var error in result.Errors)
                {
                    //displays an error message if validation fails
                    //https://docs.fluentvalidation.net/en/latest/aspnet.html

                    ModelState.AddModelError("Documents", error.ErrorMessage);
                }
                return View(claim);
            }
           
            ClaimMemory.ClaimList.Add(claim); //add claim to the ClaimList list if validation passes
            ClaimMemory.SubmittedClaim.Add(claim); //add claim to the SubmitedClaim list. this is a temporary storage location for the claims which will be removed from the list after it is verified 

            return RedirectToAction("Index"); //redirect to index once claim is submitted https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.redirecttoaction?view=aspnetcore-8.0&form=MG0AV3
        }

        [HttpGet]
        [Authorize(Roles = "Lecturer, Academic Manager")]
        public IActionResult Index()
        {
            return View(ClaimMemory.SubmittedClaim); //the claims stored in the SubmittedClaim list will be dsiplayed in the index
        }

        //method to store files in the filePaths list and authorization
        [Authorize(Roles = "Lecturer")]
        public List<string> SaveFile(List<IFormFile> files)
        {
            var filePaths = new List<string>();
            //https://learn.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=net-8.0&form=MG0AV3
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads"); //https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.getcurrentdirectory?view=net-8.0&form=MG0AV3

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath); //create directory if it doesnt already exist
            }
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName); //extract the file name from the file path and save it to fileName
                    var filePath = Path.Combine(uploadPath, file.FileName); 
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream); //https://learn.microsoft.com/en-us/dotnet/api/system.io.filestream?view=net-8.0&form=MG0AV3
                    }
                    filePaths.Add(fileName);
                }
            }
            return filePaths;
        }

        //method to display the claim history view
        //https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult?view=aspnetcore-8.0&form=MG0AV3
        [Authorize(Roles = "Lecturer")]
        public IActionResult ClaimHistory()
        {
            var history = ClaimMemory.ClaimHistory; //display claims stored in the ClaimHistory list
            return View(history);
        }

        [HttpPost]
        [Authorize(Roles = "Academic Manager, Program Coordinator")]
        public IActionResult ApproveClaim(int claimId)
        {
            var claim = ClaimMemory.ClaimList.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Approved; // Set status to Approved
            }

            return RedirectToAction("Index", "Claim");
        }
    }

}
   