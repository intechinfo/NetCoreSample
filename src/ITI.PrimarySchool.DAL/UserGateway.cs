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
                return con.Query<User>( "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken from iti.vUser u;" );
            }
        }

        public User FindById( int userId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<User>(
                        "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken from iti.vUser u where u.UserId = @UserId",
                        new { UserId = userId } )
                    .FirstOrDefault();
            }
        }

        public User FindByEmail( string email )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return con.Query<User>(
                        "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken from iti.vUser u where u.Email = @Email",
                        new { Email = email } )
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

        public void CreateGithubUser( string email, string accessToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sGithubUserCreate",
                    new { Email = email, AccessToken = accessToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void CreateGoogleUser( string email, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sGoogleUserCreate",
                    new { Email = email, RefreshToken = refreshToken },
                    commandType: CommandType.StoredProcedure );
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

        public void UpdateGithubToken( int userId, string accessToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sGithubUserUpdate",
                    new { UserId = userId, AccessToken = accessToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void UpdateGoogleToken( int userId, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sGoogleUserUpdate",
                    new { UserId = userId, refreshToken = refreshToken },
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

        public void AddGithubToken( int userId, string accessToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sUserAddGithubToken",
                    new { UserId = userId, AccessToken = accessToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public void AddGoogleToken( int userId, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sUserAddGoogleToken",
                    new { UserId = userId, RefreshToken = refreshToken },
                    commandType: CommandType.StoredProcedure );
            }
        }
    }
}
