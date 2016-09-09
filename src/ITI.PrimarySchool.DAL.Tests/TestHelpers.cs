using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ITI.PrimarySchool.DAL.Tests
{
    public static class TestHelpers
    {
        static readonly Random _random = new Random();
        static IConfiguration _configuration;

        public static string ConnectionString
        {
            get
            {
                return Configuration[ "ConnectionStrings:PrimarySchoolDB" ];
            }
        }

        static IConfiguration Configuration
        {
            get
            {
                if( _configuration == null )
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath( Directory.GetCurrentDirectory() )
                        .AddJsonFile( "appsettings.json", optional: true )
                        .AddEnvironmentVariables()
                        .Build();
                }

                return _configuration;
            }
        }

        public static string RandomTestName() => string.Format( "Test-{0}", Guid.NewGuid().ToString().Substring( 24 ) );

        public static DateTime RandomBirthDate( int age ) => DateTime.UtcNow.AddYears( -age ).AddMonths( _random.Next( -11, 0 ) ).Date;

        public static string RandomLevel()
        {
            int levelIdx = _random.Next( 5 );
            if( levelIdx == 0 ) return "CP";
            if( levelIdx == 1 ) return "CE1";
            if( levelIdx == 2 ) return "CE2";
            if( levelIdx == 3 ) return "CM1";
            return "CM2";
        }
    }
}
