using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace CMCS_Application.Controllers
{
    public class ClaimController : Controller
    {
        //generate unique claim id from 1
        private static int _claimUniqueID = 1; //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members?form=MG0AV3#static-fields
        //allowed file type definition
        private readonly string[] allowedFiles = {".pdf", ".docx", ".xlsx" }; //https://stackoverflow.com/questions/56588900/how-to-validate-uploaded-file-in-asp-net-core
        //max file size - 5mb
        private const long fileSizeLimit = 5 * 1024 * 1024; //https://stackoverflow.com/questions/38534403/how-to-always-show-the-keyboard-in-a-view-controller/38534760#38534760

        //https://stackoverflow.com/questions/61543553/how-to-add-a-button-in-a-asp-net-mvc-and-link-a-click-event-from-c-sharp-code/61543699#61543699
        [HttpGet]
        public IActionResult ClaimForm()
        {
            return View();
        }

        //https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult?view=aspnetcore-8.0&form=MG0AV3
        [HttpPost]
        public IActionResult ClaimForm(Claim claim, List<IFormFile> files)
        {
            try
            {
                if(files.Count > 5)
                {
                    throw new InvalidOperationException("Maximum number of files allowed is 5.");
                }


                claim.ClaimId = _claimUniqueID++; //auto-increment the ClaimId by 1 whenever a new claim is submitted

                foreach (var file in files)
                {
                    //check the file extension
                    if (!allowedFiles.Contains(Path.GetExtension(file.FileName).ToLower()))
                    {
                        //if the file extenstion isnt valid it will throw this error
                        ViewBag.Error = "Please note that only .pdf, .docx, and .xlsx. files are allowed."; //https://learn.microsoft.com/en-us/aspnet/core/mvc/views/overview?view=aspnetcore-8.0&form=MG0AV3#passing-data-to-views
                        return View(claim);
                    }

                    //check the file size
                    if (file.Length > fileSizeLimit)
                    {
                        //if the file size limit exceeds the limit the application will throw this error
                        ViewBag.Error = "File size must be less than 5MB.";
                        return View(claim);
                    }
                }
                //if the document matches the criteria it will be saved to the list
                claim.Documents = SaveFile(files);

                ClaimMemory.ClaimList.Add(claim); //add claim to the ClaimList list
                ClaimMemory.SubmittedClaim.Add(claim); //add claim to the SubmitedClaim list. this is a temporary storage location for the claims which will be removed from the list after it is verified 

                return RedirectToAction("Index"); //redirect to index once claim is submitted https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.redirecttoaction?view=aspnetcore-8.0&form=MG0AV3
            }

            catch (InvalidOperationException ex)
            {
                ViewBag.Error = ex.Message;
                return View(claim);
            }

            catch (Exception ex)
            {

                ViewBag.Error = "An error has occured";
                Console.WriteLine($"Error: {ex.Message}");
                return View(claim);
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(ClaimMemory.SubmittedClaim); //the claims stored in the SubmittedClaim list will be dsiplayed in the index
        }

        //method to store files in the filePaths list
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
        public IActionResult ClaimHistory()
        {
            var history = ClaimMemory.ClaimHistory; //display claims stored in the ClaimHistory list
            return View(history);
        }
    }

}
   