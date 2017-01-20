using System;
using System.Collections.Generic;
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

        public bool CreatePasswordUser( string email, string password )
        {
            if( _userGateway.FindByEmail( email ) != null ) return false;
            _userGateway.CreatePasswordUser( email, _passwordHasher.HashPassword( password ) );
            return true;
        }

        public bool CreateOrUpdateGithubUser( string email, int githubId, string accessToken )
        {
            if( _userGateway.FindByGithubId( githubId ) != null )
            {
                _userGateway.UpdateGithubToken( githubId, accessToken );
                return false;
            }
            User user = _userGateway.FindByEmail( email );
            if( user != null )
            {
                _userGateway.AddGithubToken( user.UserId, githubId, accessToken );
                return false;
            }
            _userGateway.CreateGithubUser( email, githubId, accessToken );
            return true;
        }

        public bool CreateOrUpdateGoogleUser( string email, string googleId, string refreshToken )
        {
            if( _userGateway.FindByGoogleId( googleId ) != null )
            {
                _userGateway.UpdateGoogleToken( googleId, refreshToken );
                return false;
            }
            User user = _userGateway.FindByEmail( email );
            if( user != null )
            {
                _userGateway.AddGoogleToken( user.UserId, googleId, refreshToken );
                return false;
            }
            _userGateway.CreateGoogleUser( email, googleId, refreshToken );
            return true;
        }

        public User FindUser( string email, string password )
        {
            User user = _userGateway.FindByEmail( email );
            if( user != null && _passwordHasher.VerifyHashedPassword( user.Password, password ) == PasswordVerificationResult.Success )
            {
                return user;
            }

            return null;
        }

        public User FindUser( string email )
        {
            return _userGateway.FindByEmail( email );
        }

        public User FindGoogleUser( string googleId )
        {
            return _userGateway.FindByGoogleId( googleId );
        }

        public User FindGithubUser( int githubId )
        {
            return _userGateway.FindByGithubId( githubId );
        }

        public IEnumerable<string> GetAuthenticationProviders( string userId )
        {
            return _userGateway.GetAuthenticationProviders( userId );
        }
    }
}
