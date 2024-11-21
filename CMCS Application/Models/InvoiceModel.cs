namespace CMCS_Application.Models
{
    public class InvoiceModel
    {
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal HourlyRate { get; set; }
        public int HoursWorked { get; set; }
        public string AdditionalNotes { get; set; }
        public string ClaimStatus { get; set; }
    }
}
