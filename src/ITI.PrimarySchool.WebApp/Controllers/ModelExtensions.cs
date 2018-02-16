using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Models.ClassViewModels;
using ITI.PrimarySchool.WebApp.Models.StudentViewModels;
using ITI.PrimarySchool.WebApp.Models.TeacherViewModels;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    public static class ModelExtensions
    {
        public static StudentViewModel ToStudentViewModel( this Student @this )
        {
            return new StudentViewModel
            {
                StudentId = @this.StudentId,
                FirstName = @this.FirstName,
                LastName = @this.LastName,
                BirthDate = @this.BirthDate,
                GitHubLogin = @this.GitHubLogin
            };
        }

        public static FollowedStudentViewModel ToFollowedStudentViewModel( this Student @this )
        {
            return new FollowedStudentViewModel
            {
                StudentId = @this.StudentId,
                FirstName = @this.FirstName,
                LastName = @this.LastName,
                GitHubLogin = @this.GitHubLogin
            };
        }
    }
}
