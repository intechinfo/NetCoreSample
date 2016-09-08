using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Dapper;
using System.Data;

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

        public void Create( string email, byte[] password, string githubAccessToken, string googleRefreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sUserCreate",
                    new { Email = email, Password = password, GithubAccessToken = githubAccessToken, GoogleRefreshToken = googleRefreshToken },
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

        public void Update( int userId, string email, byte[] password, string githubAccessToken, string googleRefreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                con.Execute(
                    "iti.sUserUpdate",
                    new { UserId = userId, Email = email, Password = password, GithubAccessToken = githubAccessToken, GoogleRefreshToken = googleRefreshToken },
                    commandType: CommandType.StoredProcedure );
            }
        }
    }
}
