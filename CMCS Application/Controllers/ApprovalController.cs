using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMCS_Application.Controllers
{
    //https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-9.0
    //encrypt whole view and only allow authorized individuals to access the view
    [Authorize]
    public class ApprovalController : Controller
    {
        //https://stackoverflow.com/questions/61543553/how-to-add-a-button-in-a-asp-net-mvc-and-link-a-click-event-from-c-sharp-code/61543699#61543699
        [HttpGet]
        [Authorize(Roles = "Program Coordinator, Academic Manager")]
        public IActionResult Index()
        {
            var claims = ClaimMemory.ClaimList;
            //return index view with all the claims stored in the ClaimList list
            return View(claims);
        }

        //https://stackoverflow.com/questions/54100268/mvc-approve-or-unapprove-button-clarification
        [HttpPost]
        [Authorize(Roles = "Program Coordinator, Academic Manager")]
        public IActionResult Approve(int claimID)
        {
            var claim = ClaimMemory.SubmittedClaim.FirstOrDefault(c => c.ClaimId == claimID);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Approved; //update the claim status to approved 
                ClaimMemory.ClaimList.Remove(claim); //remove the claim from the claim list list
                ClaimMemory.ClaimHistory.Add(claim); //move the claim data from the ClaimList list to the ClaimHistory list to be displayed in the claim history view
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Program Coordinator, Academic Manager")]
        public IActionResult Reject(int claimID)
        {
            var claim = ClaimMemory.ClaimList.FirstOrDefault(c => c.ClaimId == claimID);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Rejected; //update the claim status to rejected
                ClaimMemory.ClaimList.Remove(claim); //remove the claim from the claim list list
                ClaimMemory.ClaimHistory.Add(claim);//move the claim data from the ClaimList list to the ClaimHistory list to be displayed in the claim history view
            }
            return RedirectToAction("Index"); //after claim is verified, redirect to the index page
        }

    }
}
