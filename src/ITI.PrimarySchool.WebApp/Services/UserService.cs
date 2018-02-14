using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<bool> CreatePasswordUser( string email, string password )
        {
            if( await _userGateway.FindByEmail( email ) != null ) return false;
            await _userGateway.CreatePasswordUser( email, _passwordHasher.HashPassword( password ) );
            return true;
        }

        public async Task<bool> CreateOrUpdateGithubUser( string email, int githubId, string accessToken )
        {
            if( await _userGateway.FindByGithubId( githubId ) != null )
            {
                await _userGateway.UpdateGithubToken( githubId, accessToken );
                return false;
            }
            User user = await _userGateway.FindByEmail( email );
            if( user != null )
            {
                await _userGateway.AddGithubToken( user.UserId, githubId, accessToken );
                return false;
            }
            await _userGateway.CreateGithubUser( email, githubId, accessToken );
            return true;
        }

        public async Task<bool> CreateOrUpdateGoogleUser( string email, string googleId, string refreshToken )
        {
            if( await _userGateway.FindByGoogleId( googleId ) != null )
            {
                await _userGateway.UpdateGoogleToken( googleId, refreshToken );
                return false;
            }
            User user = await _userGateway.FindByEmail( email );
            if( user != null )
            {
                await _userGateway.AddGoogleToken( user.UserId, googleId, refreshToken );
                return false;
            }
            await _userGateway.CreateGoogleUser( email, googleId, refreshToken );
            return true;
        }

        public async Task<User> FindUser( string email, string password )
        {
            User user = await _userGateway.FindByEmail( email );
            if( user != null && _passwordHasher.VerifyHashedPassword( user.Password, password ) == PasswordVerificationResult.Success )
            {
                return user;
            }

            return null;
        }

        public Task<User> FindUser( string email )
        {
            return _userGateway.FindByEmail( email );
        }

        public Task<User> FindGoogleUser( string googleId )
        {
            return _userGateway.FindByGoogleId( googleId );
        }

        public Task<User> FindGithubUser( int githubId )
        {
            return _userGateway.FindByGithubId( githubId );
        }

        public Task<IEnumerable<string>> GetAuthenticationProviders( string userId )
        {
            return _userGateway.GetAuthenticationProviders( userId );
        }
    }
}
