using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class GitHubService
    {
        readonly GitHubClient _gitHubClient;
        readonly StudentGateway _studentGateway;
        readonly UserGateway _userGateway;

        public GitHubService( GitHubClient gitHubClient, StudentGateway studentGateway, UserGateway userGateway )
        {
            _gitHubClient = gitHubClient;
            _studentGateway = studentGateway;
            _userGateway = userGateway;
        }

        public async Task<Result<IEnumerable<Student>>> GetFollowedStudents( int userId )
        {
            User user = _userGateway.FindById( userId );
            if( user == null ) return Result.Failure<IEnumerable<Student>>( Status.BadRequest, "Unknown user." );
            if( user.GithubAccessToken == string.Empty ) Result.Failure<IEnumerable<Student>>( Status.BadRequest, "This user is not a known github user." );

            IEnumerable<string> logins = await _gitHubClient.GetFollowedUsers( user.GithubAccessToken );
            IEnumerable<Student> students = _studentGateway.GetByGitHubLogin( logins );

            return Result.Success( Status.Ok, students );
        }
    }
}
