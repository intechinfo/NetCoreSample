using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

        public async Task<UserData> FindById( int userId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<UserData>(
                    "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.UserId = @UserId",
                    new { UserId = userId } );
            }
        }

        public async Task<Result<UserData>> FindGitHubUser( int userId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                UserData user = await con.QueryFirstOrDefaultAsync<UserData>(
                    @"select u.UserId,
                             u.Email,
                             u.[Password],
                             u.GithubAccessToken,
                             u.GoogleRefreshToken,
                             u.GoogleId,
                             u.GithubId
                      from iti.vUser u
                      where u.UserId = @UserId;",
                    new { UserId = userId } );

                if( user == null ) return Result.Failure<UserData>( Status.BadRequest, "Unknown user." );
                if( user.GithubId == 0) return Result.Failure<UserData>( Status.BadRequest, "This user is not a known github user." );

                return Result.Success( user );
            }
        }

        public async Task<UserData> FindByEmail( string email )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<UserData>(
                    "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.Email = @Email",
                    new { Email = email } );
            }
        }

        public async Task<UserData> FindByGoogleId( string googleId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<UserData>(
                    "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.GoogleId = @GoogleId",
                    new { GoogleId = googleId } );
            }
        }

        public async Task<UserData> FindByGithubId( int githubId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<UserData>(
                    "select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken, u.GoogleId, u.GithubId from iti.vUser u where u.GithubId = @GithubId",
                    new { GithubId = githubId } );
            }
        }

        public async Task<Result<int>> CreatePasswordUser( string email, byte[] password )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@Email", email );
                p.Add( "@Password", password );
                p.Add( "@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "iti.sPasswordUserCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "An account with this email already exists." );

                Debug.Assert( status == 0 );
                return Result.Success( p.Get<int>( "@UserId" ) );
            }
        }

        public async Task CreateOrUpdateGithubUser( string email, int githubId, string accessToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sGithubUserCreateOrUpdate",
                    new { Email = email, GithubId = githubId, AccessToken = accessToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task CreateOrUpdateGoogleUser( string email, string googleId, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "iti.sGoogleUserCreateOrUpdate",
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
    }
}
