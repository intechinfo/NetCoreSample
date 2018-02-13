using System;
using NUnit.Framework;

namespace ITI.PrimarySchool.DAL.Tests
{
    [TestFixture]
    public class UserGatewayTests
    {
        readonly Random _random = new Random();

        [Test]
        public void can_create_find_update_and_delete_user()
        {
            UserGateway sut = new UserGateway( TestHelpers.ConnectionString );
            string email = string.Format( "user{0}@test.com", Guid.NewGuid() );
            byte[] password = Guid.NewGuid().ToByteArray();

            sut.CreatePasswordUser( email, password );
            User user = sut.FindByEmail( email );

            {
                Assert.That( user.Email, Is.EqualTo( email ) );
                Assert.That( user.Password, Is.EqualTo( password ) );
            }

            {
                User u = sut.FindById( user.UserId );
                Assert.That( u.Email, Is.EqualTo( email ) );
                Assert.That( u.Password, Is.EqualTo( password ) );
            }

            {
                email = string.Format( "user{0}@test.com", Guid.NewGuid() );
                sut.UpdateEmail( user.UserId, email );
            }

            {
                User u = sut.FindById( user.UserId );
                Assert.That( u.Email, Is.EqualTo( email ) );
                Assert.That( u.Password, Is.EqualTo( password ) );
            }

            {
                sut.Delete( user.UserId );
                Assert.That( sut.FindById( user.UserId ), Is.Null );
            }
        }

        [Test]
        public void can_create_github_user()
        {
            UserGateway sut = new UserGateway( TestHelpers.ConnectionString );
            string email = string.Format( "user{0}@test.com", Guid.NewGuid() );
            int githubId = _random.Next( 1000000, int.MaxValue );
            string accessToken = Guid.NewGuid().ToString().Replace( "-", string.Empty );

            sut.CreateGithubUser( email, githubId, accessToken );
            User user = sut.FindByEmail( email );

            Assert.That( user.GithubAccessToken, Is.EqualTo( accessToken ) );

            accessToken = Guid.NewGuid().ToString().Replace( "-", string.Empty );
            sut.UpdateGithubToken( user.GithubId, accessToken );

            user = sut.FindById( user.UserId );
            Assert.That( user.GithubAccessToken, Is.EqualTo( accessToken ) );

            sut.Delete( user.UserId );
        }

        [Test]
        public void can_create_google_user()
        {
            UserGateway sut = new UserGateway( TestHelpers.ConnectionString );
            string email = string.Format( "user{0}@test.com", Guid.NewGuid() );
            string googleId = Guid.NewGuid().ToString();
            string refreshToken = Guid.NewGuid().ToString().Replace( "-", string.Empty );

            sut.CreateGoogleUser( email, googleId, refreshToken );
            User user = sut.FindByEmail( email );

            Assert.That( user.GoogleRefreshToken, Is.EqualTo( refreshToken ) );

            refreshToken = Guid.NewGuid().ToString().Replace( "-", string.Empty );
            sut.UpdateGoogleToken( user.GoogleId, refreshToken );

            user = sut.FindById( user.UserId );
            Assert.That( user.GoogleRefreshToken, Is.EqualTo( refreshToken ) );

            sut.Delete( user.UserId );
        }
    }
}
