using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.PrimarySchool.DAL.Tests
{
    [TestFixture]
    public class TeacherGatewayTests
    {
        [Test]
        public async Task can_create_find_update_and_delete_teacher()
        {
            TeacherGateway sut = new TeacherGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            Result<int> result = await sut.Create( firstName, lastName );
            Assert.That( result.Status, Is.EqualTo( Status.Created ) );
            int teacherId = result.Content;

            Result<TeacherData> teacher;
            {
                teacher = await sut.FindById( teacherId );
                CheckTeacher( teacher, firstName, lastName );
            }

            {
                firstName = TestHelpers.RandomTestName();
                lastName = TestHelpers.RandomTestName();
                Result r = await sut.Update( teacherId, firstName, lastName );
                Assert.That( r.Status, Is.EqualTo( Status.Ok ) );

                teacher = await sut.FindById( teacherId );
                CheckTeacher( teacher, firstName, lastName );
            }

            {
                await sut.Delete( teacherId );
                teacher = await sut.FindById( teacherId );
                Assert.That( teacher.Status, Is.EqualTo( Status.NotFound ) );
                Assert.That( teacher.HasError, Is.True );
            }
        }

        void CheckTeacher( Result<TeacherData> teacher, string firstName, string lastName )
        {
            Assert.That( teacher.Status, Is.EqualTo( Status.Ok ) );
            Assert.That( teacher.Content.FirstName, Is.EqualTo( firstName ) );
            Assert.That( teacher.Content.LastName, Is.EqualTo( lastName ) );
        }
    }
}
