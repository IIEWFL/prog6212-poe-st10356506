using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMCS_Application.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        // Display all claims
        [Authorize(Roles = "HR")]
        public IActionResult Index()
        {
            var claims = ClaimMemory.ClaimHistory;
            return View(claims);
        }

        // Display all approved claims
        //public IActionResult ApprovedClaims()
        //{
        //    // Filter approved claims
        //    var approvedClaims = ClaimMemory.ClaimHistory
        //        .Where(c => c.Status == ClaimStatus.Approved)
        //        .ToList();

        //    return View("Index"); // Pass the filtered list to the view
        //}

        // View Invoice for a specific claim
        [Authorize(Roles = "HR")]
        public IActionResult ViewInvoice(int claimId)
        {
            // Find claim by ID in ClaimHistory
            var claim = ClaimMemory.ClaimHistory.FirstOrDefault(c => c.ClaimId == claimId);

            if (claim == null || claim.Status != ClaimStatus.Approved)
            {
                ViewBag.ErrorMessage = "No records found.";
                return View("Error");
            }

            // Set the invoice data
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

            return View(invoiceData); // Pass the invoice data to the view
        }
    }
}
