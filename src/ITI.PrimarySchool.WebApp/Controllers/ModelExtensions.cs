using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Models.StudentViewModels;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    public static class ModelExtensions
    {
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
