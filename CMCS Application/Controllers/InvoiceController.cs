using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMCS_Application.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        //display all claims, only accessible by hr
        [Authorize(Roles = "HR")]
        public IActionResult Index()
        {
            var claims = ClaimMemory.ClaimHistory;
            return View(claims);
        }

        //view invoice for selected claim
        [Authorize(Roles = "HR")]
        public IActionResult ViewInvoice(int claimId)
        {
           
            var claim = ClaimMemory.ClaimHistory.FirstOrDefault(c => c.ClaimId == claimId);

            //only display approved claims
            //https://stackoverflow.com/questions/53615076/how-to-get-a-list-of-filter-criteria-from-view-to-controller-in-asp-net-mvc
            if (claim == null || claim.Status != ClaimStatus.Approved)
            {
                ViewBag.ErrorMessage = "No records found.";
                return View("Error");
            }

            //set the invoice data and pull data from invoice model
            var invoiceData = new InvoiceModel
            {
                LecturerName = claim.LecturerName,
                LecturerEmail = claim.LecturerEmail,
                TotalAmount = claim.HoursWorked * claim.HourlyRate,
                HourlyRate = claim.HourlyRate,
                HoursWorked = claim.HoursWorked,
                AdditionalNotes = claim.AdditionalNotes,
                ClaimStatus = claim.Status.ToString(),
            };

            return View(invoiceData); //pass the invoice data to the view
        }
    }
}
