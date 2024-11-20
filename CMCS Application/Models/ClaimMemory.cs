using System.Security.Claims;

namespace CMCS_Application.Models
{
    //Lists storing the claim data submitted by the user
    public class ClaimMemory
    //https://www.tutorialsteacher.com/csharp/csharp-list
    {
        public static List<Claim> ClaimList = new List<Claim>();
        public static List<Claim> SubmittedClaim = new List<Claim>();
        public static List<Claim> ClaimHistory = new List<Claim>();
        }
    }

