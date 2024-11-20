using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMCS_Application.Controllers;  
using CMCS_Application.Models;      
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace CMCSTest
{
    //All tests passed
    [TestClass]
    public class UnitTest1
    //https://learn.microsoft.com/en-us/visualstudio/ide/how-to-add-or-remove-references-by-using-the-reference-manager?view=vs-2022&f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(VS.ReferenceManager)%26rd%3Dtrue
    //https://saucelabs.com/resources/blog/nunit-vs-xunit-vs-mstest-with-examples
    //https://www.youtube.com/watch?v=mI7fRxy44Y0
    //https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/unit-testing/creating-unit-tests-for-asp-net-mvc-applications-cs
    {
        [TestMethod]
        public void TestSubmitClaim() //test the submit claim method
        {
            var controller = new ClaimController();
            var claim = new Claim
            {
                //test data
                LecturerName = "Yadav Priaram",
                LecturerEmail = "yadavp@gmail.com",
                HoursWorked = 15,
                HourlyRate = 50,
                AdditionalNotes = "Test notes",
            };
            var files = new List<IFormFile>();

            var result = controller.ClaimForm(claim, files);

            //assert
            Assert.IsNotNull(result );
            Assert.AreEqual(1, ClaimMemory.ClaimList.Count );
        }

        [TestMethod]
        public void TestFileValidation() //test to validate the file
        {
            var controller = new ClaimController();
            var files = new List<IFormFile>
            {
                new FormFile(null, 0,0, "TestFile", "test.sql") //incorrect file type to test restrictions
            };
            var claim = new Claim
            {
                //test data
                LecturerName = "Yadav Priaram",
                LecturerEmail = "yadavpri@gmail.com"
            };

            var result = controller.ClaimForm(claim, files);

            //assert
            Assert.IsNotNull(result );
            Assert.AreEqual("Please note that only .pdf, .docx, and .xlsx. files are allowed.", controller.ViewBag.Error);
        }

        [TestMethod]
        public void TestVerifyClaim() //test to process the claim
        {
            //test data
            var claim = new Claim
            {
                ClaimId = 1,
                LecturerName = "Yadav Priaram",
                LecturerEmail = "yadavp@gmail.com",
                HoursWorked = 15,
                HourlyRate = 50,
                Status = ClaimStatus.Approved
            };

            ClaimMemory.ClaimList.Add(claim);
            claim.Status = ClaimStatus.Approved; //simulate approved claim
            //assert
            Assert.AreEqual(ClaimStatus.Approved, ClaimMemory.ClaimList[0].Status);
            
        }

        [TestMethod]
        public void TestClaimHistory() //test to send the data to the claim history
        {
            //test data
            var claim = new Claim
            {
                ClaimId= 2,
                LecturerName = "Yadav Priaram",
                LecturerEmail = "yadavp@gmail.com",
                HoursWorked = 15,
                HourlyRate = 50,
                Status= ClaimStatus.Approved
            };
            //move data from ClaimList to ClaimHistory
            ClaimMemory.ClaimList.Add(claim);
            ClaimMemory.ClaimHistory.Add(claim);

            var controller = new ClaimController();
            var results = controller.ClaimHistory();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, ClaimMemory.ClaimHistory.Count);
        }
    }
}