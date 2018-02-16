using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.PrimarySchool.DAL.Tests
{
    [TestFixture]
    public class ClassGatewayTests
    {
        [Test]
        public async Task can_create_find_update_and_delete_class()
        {
            ClassGateway sut = new ClassGateway( TestHelpers.ConnectionString );
            string name = TestHelpers.RandomTestName();
            string level = TestHelpers.RandomLevel();
            Result<int> result = await sut.Create( name, level );
            Assert.That( result.Status, Is.EqualTo( Status.Created ) );
            int classId = result.Content;

            Class c;
            Result<ClassData> classData;

            {
                c = await sut.FindByName( name );
                CheckClass( c, name, level );
            }

            {
                classData = await sut.FindById2( classId );
                CheckClass( classData, name, level );
            }

            {
                name = TestHelpers.RandomTestName();
                level = TestHelpers.RandomLevel();
                await sut.Update( classId, name, level );

                classData = await sut.FindById2( classId );
                CheckClass( classData, name, level );
            }

            {
                Result r = await sut.Delete( classId );
                Assert.That( r.Status, Is.EqualTo( Status.Ok ) );
                classData = await sut.FindById2( classId );
                Assert.That( classData.Status, Is.EqualTo( Status.NotFound ) );
            }
        }

        [Test]
        public async Task can_assign_teacher()
        {
            TeacherGateway teacherGateway = new TeacherGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            await teacherGateway.Create( firstName, lastName );
            Teacher teacher1 = await teacherGateway.FindByName( firstName, lastName );

            ClassGateway sut = new ClassGateway( TestHelpers.ConnectionString );
            string name = TestHelpers.RandomTestName();
            string level = TestHelpers.RandomLevel();
            Result<int> result = await sut.Create( name, level, teacher1.TeacherId );
            Assert.That( result.Status, Is.EqualTo( Status.Created ) );
            int classId = result.Content;

            Class c;
            {
                string firstName2 = TestHelpers.RandomTestName();
                string lastName2 = TestHelpers.RandomTestName();
                await teacherGateway.Create( firstName2, lastName2 );
                Teacher teacher2 = await teacherGateway.FindByName( firstName2, lastName2 );
                await sut.AssignTeacher( classId, teacher2.TeacherId );
                c = await sut.FindById( classId );
                CheckClass( c, name, level, teacher2.TeacherId );

                await sut.AssignTeacher( classId, 0 );
                c = await sut.FindById( classId );
                CheckClass( c, name, level, 0 );

                await teacherGateway.Delete( teacher2.TeacherId );
            }

            await sut.Delete( classId );
            await teacherGateway.Delete( teacher1.TeacherId );
        }

        void CheckClass( Result<ClassData> c, string name, string level )
        {
            Assert.That( c.HasError, Is.False );
            Assert.That( c.Status, Is.EqualTo( Status.Ok ) );
            Assert.That( c.Content.Name, Is.EqualTo( name ) );
            Assert.That( c.Content.Level, Is.EqualTo( level ) );
        }

        void CheckClass( Class c, string name, string level )
        {
            Assert.That( c.Name, Is.EqualTo( name ) );
            Assert.That( c.Level, Is.EqualTo( level ) );
        }

        void CheckClass( Class c, string name, string level, int teacherId )
        {
            CheckClass( c, name, level );
            Assert.That( c.TeacherId, Is.EqualTo( teacherId ) );
        }
    }
}
