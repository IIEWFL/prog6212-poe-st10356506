using CMCS_Application.Models;
using System.Linq;
using FluentValidation;

    public class AutoReview : AbstractValidator<Claim>
    {
        public readonly string[] _allowedExtensions = { ".pdf", ".docx", ".xlsx" }; //file types allowed
        private const long _maxFileSize = 5 * 1024 * 1024; //5 MB size limit
 
       //here i will set the rules for the claim
        public AutoReview()
        {
            //rule for hours worked with a limit of 744 hours
            RuleFor(c => c.HoursWorked)
                .LessThanOrEqualTo(744)
                .WithMessage("Hours worked must not exceed total number of hours in a month.");

            //rule for hourly rate
            RuleFor(c => c.HourlyRate)
                 .GreaterThanOrEqualTo(1)
                 .WithMessage("Hourly rate must be at least R1");

           //rule for additional notes
           RuleFor(c => c.AdditionalNotes)
                .MaximumLength(500)
                .WithMessage("Additional notes cannot exceed 500 characters in length");

            //rules for documents
           RuleFor(x => x.Documents)
            .Must(files => files == null || files.Count <= 5) //no more than 5 files
            .WithMessage("You can only upload up to 5 files.");

            RuleForEach(x => x.Documents)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024) //5 MB size limit
                .WithMessage("Each file must be less than 5MB.");

            RuleForEach(x => x.Documents)
                .Must(file => file == null || file.ContentType == "application/pdf" || file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                .WithMessage("Only .pdf, .docx, and .xlsx file types are allowed.");
        }
    }
