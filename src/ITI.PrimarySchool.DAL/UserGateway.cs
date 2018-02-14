using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<User>> GetAll()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<User>( "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u;" );
            }
        }

        public async Task<User> FindById( int userId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<User>(
                    "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.UserId = @UserId",
                    new { UserId = userId } );
            }
        }

        public async Task<User> FindByEmail( string email )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<User>(
                    "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.Email = @Email",
                    new { Email = email } );
            }
        }

        public async Task<User> FindByGoogleId( string googleId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<User>(
                    "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.GoogleId = @GoogleId",
                    new { GoogleId = googleId } );
            }
        }

        public async Task<User> FindByGithubId( int githubId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<User>(
                    "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.GithubId = @GithubId",
                    new { GithubId = githubId } );
            }
        }

        public async Task CreatePasswordUser( string email, byte[] password )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sPasswordUserCreate",
                    new { Email = email, Password = password },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task CreateGithubUser( string email, int githubId, string accessToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sGithubUserCreate",
                    new { Email = email, GithubId = githubId, AccessToken = accessToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task CreateGoogleUser( string email, string googleId, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sGoogleUserCreate",
                    new { Email = email, GoogleId = googleId, RefreshToken = refreshToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task<IEnumerable<string>> GetAuthenticationProviders( string userId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<string>(
                    "select p.ProviderName from iti.vAuthenticationProvider p where p.UserId = @UserId",
                    new { UserId = userId } );
            }
        }

        public async Task Delete( int userId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync( "iti.sUserDelete", new { UserId = userId }, commandType: CommandType.StoredProcedure );
            }
        }

        public async Task UpdateEmail( int userId, string email )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sUserUpdate",
                    new { UserId = userId, Email = email },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task UpdatePassword( int userId, byte[] password )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sPasswordUserUpdate",
                    new { UserId = userId, Password = password },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task UpdateGithubToken( int githubId, string accessToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sGithubUserUpdate",
                    new { GithubId = githubId, AccessToken = accessToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task UpdateGoogleToken( string googleId, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sGoogleUserUpdate",
                    new { GoogleId = googleId, RefreshToken = refreshToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task AddPassword( int userId, byte[] password )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sUserAddPassword",
                    new { UserId = userId, Password = password },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task AddGithubToken( int userId, int githubId, string accessToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sUserAddGithubToken",
                    new { UserId = userId, GithubId = githubId, AccessToken = accessToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task AddGoogleToken( int userId, string googleId, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sUserAddGoogleToken",
                    new { UserId = userId, GoogleId = googleId, RefreshToken = refreshToken },
                    commandType: CommandType.StoredProcedure );
            }
        }
    }
}
