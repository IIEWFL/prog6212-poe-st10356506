namespace CMCS_Application.Models
{
    //list for storing the invoice data
    //https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-8.0&tabs=visual-studio
    //https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-8.0&form=MG0AV3
    public class InvoiceModel
    {
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }
        public double TotalAmount { get; set; }
        public decimal HourlyRate { get; set; }
        public int HoursWorked { get; set; }
        public string AdditionalNotes { get; set; }
        public string ClaimStatus { get; set; }
    }
}
