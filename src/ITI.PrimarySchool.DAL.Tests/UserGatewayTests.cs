using System;
using NUnit.Framework;

namespace ITI.PrimarySchool.DAL.Tests
{
    [TestFixture]
    public class UserGatewayTests
    {
        [Test]
        public void can_create_find_update_and_delete_user()
        {
            UserGateway sut = new UserGateway( TestHelpers.ConnectionString );
            string email = string.Format( "user{0}@test.com", Guid.NewGuid() );
            string password = Guid.NewGuid().ToString().Substring( 0, 32 );

            sut.Create( email, password );
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
                password = Guid.NewGuid().ToString().Substring( 0, 32 );
                sut.Update( user.UserId, email, password );
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
    }
}
