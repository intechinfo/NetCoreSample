using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace ITI.PrimarySchool.DAL
{
    public class UserGateway
    {
        readonly string _connectionString;

        public UserGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public IEnumerable<User> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<User>( "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u;" );
            }
        }

        public User FindById( int userId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<User>(
                        "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.UserId = @UserId",
                        new { UserId = userId } )
                    .FirstOrDefault();
            }
        }

        public User FindByEmail( string email )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<User>(
                        "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.Email = @Email",
                        new { Email = email } )
                    .FirstOrDefault();
            }
        }

        public User FindByGoogleId( string googleId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<User>(
                        "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.GoogleId = @GoogleId",
                        new { GoogleId = googleId } )
                    .FirstOrDefault();
            }
        }

        public User FindByGithubId( int githubId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<User>(
                        "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.GithubId = @GithubId",
                        new { GithubId = githubId } )
                    .FirstOrDefault();
            }
        }

        public void CreatePasswordUser( string email, byte[] password )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sPasswordUserCreate",
                    new { Email = email, Password = password },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void CreateGithubUser( string email, int githubId, string accessToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sGithubUserCreate",
                    new { Email = email, GithubId = githubId, AccessToken = accessToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void CreateGoogleUser( string email, string googleId, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sGoogleUserCreate",
                    new { Email = email, GoogleId = googleId, RefreshToken = refreshToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public IEnumerable<string> GetAuthenticationProviders( string userId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<string>(
                    "select p.ProviderName from iti.vAuthenticationProvider p where p.UserId = @UserId",
                    new { UserId = userId } );
            }
        }

        public void Delete( int userId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute( "iti.sUserDelete", new { UserId = userId }, commandType: CommandType.StoredProcedure );
            }
        }

        public void UpdateEmail( int userId, string email )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sUserUpdate",
                    new { UserId = userId, Email = email },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void UpdatePassword( int userId, byte[] password )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sPasswordUserUpdate",
                    new { UserId = userId, Password = password },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void UpdateGithubToken( int githubId, string accessToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sGithubUserUpdate",
                    new { GithubId = githubId, AccessToken = accessToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void UpdateGoogleToken( string googleId, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sGoogleUserUpdate",
                    new { GoogleId = googleId, RefreshToken = refreshToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void AddPassword( int userId, byte[] password )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sUserAddPassword",
                    new { UserId = userId, Password = password },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void AddGithubToken( int userId, int githubId, string accessToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sUserAddGithubToken",
                    new { UserId = userId, GithubId = githubId, AccessToken = accessToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void AddGoogleToken( int userId, string googleId, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sUserAddGoogleToken",
                    new { UserId = userId, GoogleId = googleId, RefreshToken = refreshToken },
                    commandType: CommandType.StoredProcedure );
            }
        }
    }
}
