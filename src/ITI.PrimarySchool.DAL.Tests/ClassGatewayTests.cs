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
            await sut.Create( name, level );
            Class c;

            {
                c = await sut.FindByName( name );
                CheckClass( c, name, level );
            }

            {
                c = await sut.FindById( c.ClassId );
                CheckClass( c, name, level );
            }

            {
                name = TestHelpers.RandomTestName();
                level = TestHelpers.RandomLevel();
                await sut.Update( c.ClassId, name, level );

                c = await sut.FindById( c.ClassId );
                CheckClass( c, name, level );
            }

            {
                await sut.Delete( c.ClassId );
                c = await sut.FindById( c.ClassId );
                Assert.That( c, Is.Null );
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
            await sut.Create( name, level, teacher1.TeacherId );

            Class c;

            {
                c = await sut.FindByName( name );
                CheckClass( c, name, level, teacher1.TeacherId );
            }

            {
                string firstName2 = TestHelpers.RandomTestName();
                string lastName2 = TestHelpers.RandomTestName();
                await teacherGateway.Create( firstName2, lastName2 );
                Teacher teacher2 = await teacherGateway.FindByName( firstName2, lastName2 );
                await sut.AssignTeacher( c.ClassId, teacher2.TeacherId );
                c = await sut.FindById( c.ClassId );
                CheckClass( c, name, level, teacher2.TeacherId );

                await sut.AssignTeacher( c.ClassId, 0 );
                c = await sut.FindById( c.ClassId );
                CheckClass( c, name, level, 0 );

                await teacherGateway.Delete( teacher2.TeacherId );
            }

            await sut.Delete( c.ClassId );
            await teacherGateway.Delete( teacher1.TeacherId );
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
