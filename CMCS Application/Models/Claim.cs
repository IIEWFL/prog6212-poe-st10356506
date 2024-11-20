using CMCS_Application.Models;

//Claim model containing variables for the claim data to be stored 
namespace CMCS_Application.Models
{
    //https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-8.0&tabs=visual-studio
    //https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-8.0&form=MG0AV3
    public class Claim
    {
    public int ClaimId { get; set; }
    public string LecturerName { get; set;}
    public string LecturerEmail { get; set;}
    public int HoursWorked { get; set; }
    public int HourlyRate { get; set; }
    public string AdditionalNotes { get; set; }
    public ClaimStatus Status { get; set; } = ClaimStatus.Pending; //default status for an unverified claim
        public List<string> Documents { get; set; } = new List<string>();
    }
    public enum ClaimStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
