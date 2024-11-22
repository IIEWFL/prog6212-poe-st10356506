namespace CMCS_Application.Models
{
    //list for storing lecturer data
    //https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-8.0&tabs=visual-studio
    //https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-8.0&form=MG0AV3
    public class Lecturer
    {
        public int Id { get; set; }
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }
        public string Role { get; set; }
    }
}
