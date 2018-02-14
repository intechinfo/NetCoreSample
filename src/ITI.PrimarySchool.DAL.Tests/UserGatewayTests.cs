using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.PrimarySchool.DAL.Tests
{
    [TestFixture]
    public class UserGatewayTests
    {
        readonly Random _random = new Random();

        [Test]
        public async Task can_create_find_update_and_delete_user()
        {
            UserGateway sut = new UserGateway( TestHelpers.ConnectionString );
            string email = string.Format( "user{0}@test.com", Guid.NewGuid() );
            byte[] password = Guid.NewGuid().ToByteArray();

            await sut.CreatePasswordUser( email, password );
            User user = await sut.FindByEmail( email );

            {
                Assert.That( user.Email, Is.EqualTo( email ) );
                Assert.That( user.Password, Is.EqualTo( password ) );
            }

            {
                User u = await sut.FindById( user.UserId );
                Assert.That( u.Email, Is.EqualTo( email ) );
                Assert.That( u.Password, Is.EqualTo( password ) );
            }

            {
                email = string.Format( "user{0}@test.com", Guid.NewGuid() );
                await sut.UpdateEmail( user.UserId, email );
            }

            {
                User u = await sut.FindById( user.UserId );
                Assert.That( u.Email, Is.EqualTo( email ) );
                Assert.That( u.Password, Is.EqualTo( password ) );
            }

            {
                await sut.Delete( user.UserId );
                Assert.That( await sut.FindById( user.UserId ), Is.Null );
            }
        }

        [Test]
        public async Task can_create_github_user()
        {
            UserGateway sut = new UserGateway( TestHelpers.ConnectionString );
            string email = string.Format( "user{0}@test.com", Guid.NewGuid() );
            int githubId = _random.Next( 1000000, int.MaxValue );
            string accessToken = Guid.NewGuid().ToString().Replace( "-", string.Empty );

            await sut.CreateGithubUser( email, githubId, accessToken );
            User user = await sut.FindByEmail( email );

            Assert.That( user.GithubAccessToken, Is.EqualTo( accessToken ) );

            accessToken = Guid.NewGuid().ToString().Replace( "-", string.Empty );
            await sut.UpdateGithubToken( user.GithubId, accessToken );

            user = await sut.FindById( user.UserId );
            Assert.That( user.GithubAccessToken, Is.EqualTo( accessToken ) );

            await sut.Delete( user.UserId );
        }

        [Test]
        public async Task can_create_google_user()
        {
            UserGateway sut = new UserGateway( TestHelpers.ConnectionString );
            string email = string.Format( "user{0}@test.com", Guid.NewGuid() );
            string googleId = Guid.NewGuid().ToString();
            string refreshToken = Guid.NewGuid().ToString().Replace( "-", string.Empty );

            await sut.CreateGoogleUser( email, googleId, refreshToken );
            User user = await sut.FindByEmail( email );

            Assert.That( user.GoogleRefreshToken, Is.EqualTo( refreshToken ) );

            refreshToken = Guid.NewGuid().ToString().Replace( "-", string.Empty );
            await sut.UpdateGoogleToken( user.GoogleId, refreshToken );

            user = await sut.FindById( user.UserId );
            Assert.That( user.GoogleRefreshToken, Is.EqualTo( refreshToken ) );

            await sut.Delete( user.UserId );
        }
    }
}
