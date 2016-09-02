using ITI.PrimarySchool.DAL;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class UserService
    {
        readonly UserGateway _userGateway;
        readonly PasswordHasher _passwordHasher;

        public UserService( UserGateway userGateway, PasswordHasher passwordHasher )
        {
            _userGateway = userGateway;
            _passwordHasher = passwordHasher;
        }

        public bool CreateUser( string email, string password )
        {
            if( _userGateway.FindByEmail( email ) != null ) return false;
            _userGateway.Create( email, _passwordHasher.HashPassword( password ), string.Empty );
            return true;
        }

        public bool CreateOrUpdateGithubUser( string email, string accessToken )
        {
            User user = _userGateway.FindByEmail( email );
            if( user != null )
            {
                _userGateway.Update( user.UserId, email, user.Password, accessToken );
                return false;
            }

            _userGateway.Create( email, new byte[ 0 ], accessToken );
            return true;
        }

        public User FindUser( string email, string password )
        {
            User user = _userGateway.FindByEmail( email );
            if( user != null && _passwordHasher.VerifyHashedPassword( user.Password, password ) == PasswordVerificationResult.Success )
            {
                return FindUser( email );
            }

            return null;
        }

        public User FindUser( string email )
        {
            return _userGateway.FindByEmail( email );
        }
    }
}
