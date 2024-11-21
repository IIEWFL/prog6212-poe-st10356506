using Microsoft.AspNetCore.Mvc;
using CMCS_Application.Models;

namespace CMCS_Application.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult ViewInvoice(int claimId)
        {
            var claim = ClaimMemory.ClaimList.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim == null || claim.Status != ClaimStatus.Approved)
            {
                return NotFound();
            }

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

            return View(invoiceData);
        }
    }
}
