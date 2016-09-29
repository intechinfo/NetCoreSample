using System;

namespace ITI.PrimarySchool.WebApp.Models.StudentViewModels
{
    public class FollowedStudentViewModel
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string GitHubLogin { get; set; }
    }
}
