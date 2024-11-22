using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;

namespace CMCS_Application.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            var claims = ClaimMemory.ClaimList;
            return View(claims);
        }

        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult ViewInvoice(int claimId)
        {
            var claim = ClaimMemory.ClaimHistory.FirstOrDefault(c => c.ClaimId == claimId);

            if (claim == null || claim.Status != ClaimStatus.Approved)
            {
                ViewBag.ErrorMessage = "No records found.";
                return View("Error");
            }
            var approvedClaims = ClaimMemory.ClaimHistory.Where(c => c.Status == ClaimStatus.Approved).ToList();
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

            return View(approvedClaims);
        }
    }
}
